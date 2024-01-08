using Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TicketFileRepository : ITicketRepository
    {
        private string _filePath;
        private List<Ticket> _tickets;

        public TicketFileRepository(string filePath)
        {
            _filePath = filePath;
            LoadTickets();
        }

        private void LoadTickets()
        {
            if (!File.Exists(_filePath))
            {
                _tickets = new List<Ticket>();
                SaveTickets(); 
            }
            else
            {
                try
                {
                    string json = File.ReadAllText(_filePath);
                    _tickets = JsonSerializer.Deserialize<List<Ticket>>(json) ?? new List<Ticket>();
                }
                catch
                {
                    
                    _tickets = new List<Ticket>();
                }
            }
        }

        private void SaveTickets()
        {
            string json = JsonSerializer.Serialize(_tickets);
            File.WriteAllText(_filePath, json);
            Debug.WriteLine(json);
         
        }

        public Ticket GetTicket(int ticketId)
        {
            return _tickets.FirstOrDefault(t => t.Id == ticketId);
        }
        public Ticket GetTicketBySeat(int flightId, string row, string column)
        {
            LoadTickets(); 
            return _tickets.FirstOrDefault(t => t.FlightIdFk == flightId && t.Row == row && t.Column == column);
        }

        public IEnumerable<Ticket> GetAllTickets() {  return _tickets; }

        public async Task<Ticket> GetTicketAsync(int ticketId)
        {
            return await Task.FromResult(_tickets.FirstOrDefault(t => t.Id == ticketId));
        }

        public void BookTicket(Ticket ticket)
        {
            if (_tickets.Any(t => t.FlightIdFk == ticket.FlightIdFk && t.Row == ticket.Row && t.Column == ticket.Column))
            {
                throw new InvalidOperationException("Seat already booked");
            }
            _tickets.Add(ticket);
            SaveTickets();
        }

        public IEnumerable<Ticket> GetTicketsByPassportNumber(string passportNumber)
        {
            return _tickets.Where(t => t.PassportNumber == passportNumber).ToList();
        }

   

    }
}
