using Microsoft.EntityFrameworkCore;

namespace Ticket_Booking_App.Models
{
    public class EventManagementBDContext:DbContext
    {
        public EventManagementBDContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<EventManagement> EventManagements { get; set; }
        public DbSet<TicketManagement> TicketManagements { get; set; }
    }
}
