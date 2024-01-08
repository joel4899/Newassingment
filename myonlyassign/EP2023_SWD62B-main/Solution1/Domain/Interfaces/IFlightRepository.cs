using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;  

namespace DataAccess.Repositories
{
    public interface IFlightRepository
    {
        // method to get a specific flight by its ID
        Flight GetFlight(int flightId);

        // method to get all flights in the database
        IEnumerable<Flight> GetFlights();
    }
}

