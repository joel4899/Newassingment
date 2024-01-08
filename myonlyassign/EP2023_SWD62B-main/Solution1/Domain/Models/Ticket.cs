using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Ticket
    {
        [Key]

        public int Id { get; set; }
     
        public string Row { get; set; }

        public string Column { get; set; }

        [ForeignKey("Flight")]
        public int FlightIdFk { get; set; }

        public virtual Flight Flight { get; set; }
        public string PassportNumber { get; set; }
        public decimal PricePaid { get; set; }
        public string? Image { get; set; }
        public bool Cancelled { get; set; }

    }
}   