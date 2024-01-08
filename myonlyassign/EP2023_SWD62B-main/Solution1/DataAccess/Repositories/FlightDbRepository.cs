using DataAccess.DataContext;
using DataAccess;
using Domain;

namespace DataAccess.Repositories
{
    public class FlightDbRepository : IFlightRepository
    {
        private readonly AirlineDbContext _context;

        public FlightDbRepository(AirlineDbContext context)
        {
            _context = context;
        }

        public Flight GetFlight(int flightId) 
        {
            return _context.Flights.FirstOrDefault(f => f.Id == flightId);
            
        }

        public List<Flight> GetAllFlights()
        {
            return _context.Flights.ToList();
        }

        public IEnumerable<Flight> GetFlights()
        {
            return _context.Flights.ToList();
        }

        
    }
}
