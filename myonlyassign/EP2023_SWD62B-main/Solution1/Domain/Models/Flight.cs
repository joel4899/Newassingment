using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System;
namespace Domain
{
    public class Flight
    {
        public int Capacity { get; set; }  // Represents the total seats available on the flight

       
        public virtual ICollection<Ticket> Tickets { get; set; }

        public bool IsFullyBooked()
        {
            // Check if the Tickets collection is null
            if (this.Tickets == null)
            {
               
                return false;
            }

            // Now it's safe to use Tickets as it's confirmed to be not null
            return Tickets.Count >= Capacity;
        }

        [Key]
        public int Id { get; set; }

        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string Rows { get; set; }

        public string Columns { get; set; }
        public decimal WholesalePrice { get; set; }
        public double ComissionRate { get; set; }

    }
}