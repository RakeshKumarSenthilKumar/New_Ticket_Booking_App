using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ticket_Booking_App.Models
{
    public class EventManagement

    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string EventName { get; set; } = "";

        //date
        public DateTime Date { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Venue { get; set; } = "";

        [Column(TypeName = "int")]
        public int TotalSeats { get; set; } = 0;

        [Column(TypeName = "int")]
        public int AvailableSeats { get; set; } = 0;
    }
    public class TicketManagement
    {
        public int Id { get; set; }
        //  public int EventId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string EventName { get; set; } = "";

        [Column(TypeName = "nvarchar(100)")]
        public string UserName { get; set; } = "";

        public int Tickets { get; set; } = 0;

        [Column(TypeName = "nvarchar(100)")]
        public string BookingReference { get; set; } = "";
    }
    public class TicketBookingRequest
    {
        public string EventName { get; set; }
        public string UserName { get; set; }
        public int Tickets { get; set; }
    }
    public class TicketCancel
    {
        public string Referrence { get; set; }
    }
}
