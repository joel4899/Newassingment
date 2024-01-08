using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
 
    public class AdminController : Controller
    {

        private readonly IFlightRepository _flightRepository;
        private readonly ITicketRepository _ticketRepository;

        public AdminController(IFlightRepository flightRepository, ITicketRepository ticketRepository)
        {
            _flightRepository = flightRepository;
            _ticketRepository = ticketRepository;
        }

       

        public async Task<IActionResult> ViewTickets(int flightId)
        {
            var tickets = _ticketRepository.GetAllTickets(); 
            return View(tickets);
        }

        public async Task<IActionResult> ViewTicketDetails(int ticketId)
        {
            var ticket = await _ticketRepository.GetTicketAsync(ticketId);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        public IActionResult Index()
        {

        
          

            if (
                    User.Identity.IsAuthenticated == false)
            {

                TempData["error"] = "Äccess Denied";

                return RedirectToAction("Index", "Home");
                
            }
            var flights =  _flightRepository.GetFlights(); 
            return View(flights);
           
        }


      
    }
}
