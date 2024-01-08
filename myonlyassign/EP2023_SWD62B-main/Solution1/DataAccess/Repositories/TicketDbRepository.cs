using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using Domain;
using DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class TicketDbRepository : ITicketRepository
    {
        private readonly AirlineDbContext _context;

        public TicketDbRepository(AirlineDbContext context)
        {
            _context = context;
        }
        public Ticket GetTicketBySeat(int flightId, string row, string column)
        {
            return _context.Tickets
                           .FirstOrDefault(t => t.FlightIdFk == flightId && t.Row == row && t.Column == column);
        }

        public Ticket GetTicket(int ticketId)
        {
            return _context.Tickets.FirstOrDefault(t => t.Id == ticketId);
        }
        public void Book(int flightId, string row, string column, string passport , string image )
        {
            // Check if the flight exists
            var flight = _context.Flights.FirstOrDefault(f => f.Id == flightId);
            if (flight == null)
            {
                throw new InvalidOperationException("Flight does not exist");
            }

          
            var existingTicket = _context.Tickets.FirstOrDefault(t => t.FlightIdFk == flightId && t.Row == row && t.Column == column);
            if (existingTicket != null)
            {
                throw new InvalidOperationException("Seat already booked");
            }

            
            var newTicket = new Ticket
            {
                FlightIdFk = flightId,
                Row = row,
                Column = column,
                PassportNumber = passport,
               Image = image,
                Cancelled = false 
            };

            // Add the new ticket to the context
            _context.Tickets.Add(newTicket);

          
            _context.SaveChanges();
        }

        public IEnumerable<Ticket> GetTicketsByPassportNumber(string passportNumber)
        {
            return _context.Tickets.Where(t => t.PassportNumber == passportNumber).ToList();
        }
        public async Task<Ticket> GetTicketAsync(int ticketId)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
        }
        public IEnumerable<Ticket> GetTickets(int flightId)
        {
           
            var tickets = _context.Tickets.Where(t => t.FlightIdFk == flightId).ToList();
            return tickets;
        }
        public IEnumerable<Ticket> GetAllTickets()
        {
            var tickets = _context.Tickets;
            return tickets;
        }

        public void Cancel(int ticketId)
        {
            // Find the ticket
            var ticket = _context.Tickets.FirstOrDefault(t => t.Id == ticketId);

            
            if (ticket == null)
            {
                throw new InvalidOperationException("Ticket not found");
            }

           
            ticket.Cancelled = true;

           
            _context.SaveChanges();
        }

        public void BookTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
        }

       
    }
}