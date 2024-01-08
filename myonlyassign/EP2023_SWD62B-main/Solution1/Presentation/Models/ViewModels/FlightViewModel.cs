namespace presentation.Models.ViewModels
{
    public class FlightViewModel
    {
        public int FlightId { get; set; }
       
        public DateTime DepartureDate { get; set; }
        public bool IsFullyBooked { get; set; }
        public decimal WholeSalePrice { get; set; }
    }
}
