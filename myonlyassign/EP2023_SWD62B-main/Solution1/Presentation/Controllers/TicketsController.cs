using Domain;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity; 
using presentation.Models.ViewModels;
using Presentation.Models;
using System.Security.Claims;
using System.Diagnostics; 
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace presentation.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly UserManager<CustomUser> _userManager; 

        public TicketsController(
            IFlightRepository flightRepository,
            ITicketRepository ticketRepository,
            UserManager<CustomUser> userManager) 
        {
            _flightRepository = flightRepository;
            _ticketRepository = ticketRepository;
            _userManager = userManager; 
        }

        public ActionResult ListFlights()
        {
            var flights = _flightRepository.GetFlights()
                            .Where(f => f.DepartureDate > DateTime.Now) 
                            .Select(f => new FlightViewModel
                            {
                                FlightId = f.Id,
                                DepartureDate = f.DepartureDate,
                                IsFullyBooked = f.IsFullyBooked(), 
                                WholeSalePrice = f.WholesalePrice
                            }).ToList();

            return View(flights); 
        }

        public async Task<ActionResult> BookFlight(int flightId)
        {
            var flight = _flightRepository.GetFlight(flightId);



            if (flight == null || flight.IsFullyBooked() || flight.DepartureDate <= DateTime.Now)
            {

                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            if (flight != null)
            {
                Debug.WriteLine($"Flight ID: {flight.Id}, Departure: {flight.DepartureDate}, IsFullyBooked: {flight.IsFullyBooked()}"); // Debug line
            }

            var model = new BookFlightViewModel
            {
                FlightId = flightId,
                PricePaid = CalculateRetailPrice(flight.WholesalePrice), 
            };

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                Debug.WriteLine($"User is authenticated. User ID: {user?.Id}"); // Debug line
                model.PassportNumber = user?.PassportNumber; 
            }
            else
            {
                Debug.WriteLine("User is not authenticated."); // Debug line
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BookFlight(BookFlightViewModel model,  [FromServices] IWebHostEnvironment host)
        {
            Debug.WriteLine("Starting BookFlight POST method.");
            var flight = _flightRepository.GetFlight(model.FlightId);
            if (flight == null || flight.IsFullyBooked() || flight.DepartureDate <= DateTime.Now)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            var existingTicket = _ticketRepository.GetTicketBySeat(model.FlightId, model.Row, model.Column);
            if (existingTicket != null && !existingTicket.Cancelled)
            {
                ModelState.AddModelError("", "This seat has already been booked.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            Debug.WriteLine($"[HttpPost BookFlight] Received Price Paid: {model.Imagefile}");
            if (!ModelState.IsValid)
            {
                // Check for errors
                foreach (var error in ModelState)
                {
                    Debug.WriteLine($"Error in {error.Key}: {error.Value.Errors.FirstOrDefault()?.ErrorMessage}");
                }

                return View(model); 
            }
            string relativePath = "";
            //upload of an image
            if (model.Imagefile != null)
            {
               
                string newFilename = Guid.NewGuid().ToString()
                    + Path.GetExtension(model.Imagefile.FileName); 

                relativePath = "/images/" + newFilename;

          
                string absolutePath = host.WebRootPath + "\\images\\" + newFilename;

             
                using (FileStream fs = new FileStream(absolutePath, FileMode.CreateNew))
                {
                    model.Imagefile.CopyTo(fs);
                    fs.Flush();
                }
            }
                Debug.WriteLine("Creating ticket entity.");
                var ticket = new Ticket
                {
                    FlightIdFk = model.FlightId,
                    Column = model.Column,
                    Row = model.Row,
                    PassportNumber = model.PassportNumber,
                    PricePaid = model.PricePaid,
                    Image = relativePath
                    
                };
                Debug.WriteLine ("Saving ticket to the repository.");
                _ticketRepository.BookTicket(ticket);
                Debug.WriteLine($"Ticket booked successfully: TicketId={ticket.Id}");

                return RedirectToAction("BookingConfirmation", new { ticketId = ticket.Id });
            }
          
        
        public IActionResult BookingConfirmation(int ticketId)
        {
            var ticket = _ticketRepository.GetTicket(ticketId);
            if (ticket == null)
            {
                return View("Error");
            }

            var viewModel = new BookingConfirmationViewModel
            {
                FlightIdFk = ticket.FlightIdFk,
                Row = ticket.Row,
                Column = ticket.Column,
                PassportNumber = ticket.PassportNumber,
                PricePaid = ticket.PricePaid,
                Image= ticket.Image
            };

            return View(viewModel);
        }

        private decimal CalculateRetailPrice(decimal wholesalePrice)
        {
           
            const decimal commissionRate = 0.1m;
            var retailPrice = wholesalePrice * (1 + commissionRate);

            return retailPrice;
        }
        public async Task<IActionResult> PastPurchases()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userManager.GetUserAsync(User);
            var passportNumber = user?.PassportNumber;
            if (string.IsNullOrEmpty(passportNumber))
            {
                
                return View("Error");
            }

            var tickets = _ticketRepository.GetTicketsByPassportNumber(passportNumber);
            return View(tickets);
        }


        public IActionResult TicketDetails(int ticketId)
        {
            var ticket = _ticketRepository.GetTicket(ticketId);
            if (ticket == null)
            {
                return NotFound(); 
            }

            var user = _userManager.GetUserAsync(User).Result; 
            if (user == null || ticket.PassportNumber != user.PassportNumber)
            {
                return NotFound(); 
            }

            return View(ticket);
        }


        
    }
}

