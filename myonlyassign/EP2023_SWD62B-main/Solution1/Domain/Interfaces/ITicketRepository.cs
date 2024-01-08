using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface ITicketRepository
    {
        Ticket GetTicket(int ticketId);
        void BookTicket(Ticket ticket);
 
        Task<Ticket> GetTicketAsync(int ticketId);
        IEnumerable<Ticket> GetAllTickets();
        IEnumerable<Ticket> GetTicketsByPassportNumber(string passportNumber);
        Ticket GetTicketBySeat(int flightId, string row, string column);

    }
    
}
