using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticket_Booking_App.Models;

namespace Ticket_Booking_App.Services
{
    public class EventManagementService
    {
        private readonly EventManagementBDContext _context;

        public EventManagementService(EventManagementBDContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventManagement>> GetEventsAsync()
        {
            return await _context.EventManagements.ToListAsync();
        }

        public async Task<EventManagement> CreateEventAsync(EventManagement eventManagement)
        {
            _context.EventManagements.Add(eventManagement);
            await _context.SaveChangesAsync();
            return eventManagement;
        }

        public async Task<IEnumerable<TicketManagement>> GetTicketsAsync()
        {
            return await _context.TicketManagements.ToListAsync();
        }

        public async Task<TicketManagement> BookTicketsAsync(TicketBookingRequest request)
        {
            var evnt = await _context.EventManagements.FirstOrDefaultAsync(e => e.EventName == request.EventName);
            if (evnt == null || evnt.AvailableSeats < request.Tickets)
                return null;

            evnt.AvailableSeats -= request.Tickets;
            var booking = new TicketManagement
            {
                EventName = evnt.EventName,
                UserName = request.UserName,
                Tickets = request.Tickets,
                BookingReference = Guid.NewGuid().ToString()
            };

            _context.TicketManagements.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> CancelBookingAsync(string reference)
        {
            var booking = await _context.TicketManagements.FirstOrDefaultAsync(b => b.BookingReference == reference);
            if (booking == null) return false;

            var evnt = await _context.EventManagements.FindAsync(booking.Id);
            if (evnt != null) evnt.AvailableSeats += booking.Tickets;

            _context.TicketManagements.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
