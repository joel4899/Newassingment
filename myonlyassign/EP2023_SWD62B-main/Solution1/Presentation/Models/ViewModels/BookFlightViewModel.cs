using System.ComponentModel.DataAnnotations;

namespace presentation.Models.ViewModels
{
    public class BookFlightViewModel
    {
       
            // Flight details
            public int FlightId { get; set; }
           // Assuming passenger must specify these
            public string Row { get; set; }

           
            public string Column { get; set; }

          
            [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; } 
        public decimal PricePaid { get; set; }

        public IFormFile Imagefile { get; set; }


    }

    }

