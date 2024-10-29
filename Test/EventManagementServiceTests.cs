using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Ticket_Booking_App.Models;
using Ticket_Booking_App.Services;

namespace Ticket_Booking_App.Tests.Services
{
    [TestFixture]
    public class EventManagementServiceTests
    {
        private EventManagementBDContext _context;
        
        private EventManagementService _service;


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EventManagementBDContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new EventManagementBDContext(options);
            _service = new EventManagementService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            
            _context.Dispose();
        }

        [Test, Order(1)]
        public async Task GetEventsAsync_ReturnsAllEvents()
        {
            
            var events = new List<EventManagement>
            {
                new EventManagement { Id = 4, EventName = "Concert", Date = DateTime.Now, Venue = "Stadium", TotalSeats = 100, AvailableSeats = 50 },
                new EventManagement { Id = 5, EventName = "Theater", Date = DateTime.Now.AddDays(1), Venue = "Theater Hall", TotalSeats = 200, AvailableSeats = 150 }
            };
            await _context.EventManagements.AddRangeAsync(events);
            await _context.SaveChangesAsync();

           
            var result = await _service.GetEventsAsync();

            
            Assert.AreEqual(2, result.Count());
        }



        [Test, Order(3)]
        public async Task BookTicketsAsync_ReturnsBooking_WhenSuccessful()
        {
            // Arrange
            var evnt = new EventManagement { Id = 1, EventName = "Concert", Date = DateTime.Now, Venue = "Stadium", TotalSeats = 100, AvailableSeats = 50 };
            await _context.EventManagements.AddAsync(evnt);
            await _context.SaveChangesAsync();

            var request = new TicketBookingRequest { EventName = "Concert", UserName = "John", Tickets = 2 };

            // Act
            var booking = await _service.BookTicketsAsync(request);
            //evnt.AvailableSeats -= request.Tickets;
            // Assert
            Assert.IsNotNull(booking);
            Assert.AreEqual("Concert", booking.EventName);
            Assert.AreEqual(2, booking.Tickets);
            
        }

        [Test, Order(4)]
        public async Task BookTicketsAsync_ReturnsNull_WhenNotEnoughSeatsAvailable()
        {
            // Arrange
            var evnt = new EventManagement { Id = 6, EventName = "Concert", Date = DateTime.Now, Venue = "Stadium", TotalSeats = 100, AvailableSeats = 1 };
            await _context.EventManagements.AddAsync(evnt);
            await _context.SaveChangesAsync();

            var request = new TicketBookingRequest { EventName = "Concert", UserName = "John", Tickets = 2 }; 

            // Act
            var booking = await _service.BookTicketsAsync(request);

            // Assert
            
            Assert.IsNull(booking);
           
        }

        [Test, Order(5)]
        public async Task CancelBookingAsync_ReturnsFalse_WhenBookingNotFound()
        {
            // Arrange
            var reference = "nonexistent"; 
            // Act
            var result = await _service.CancelBookingAsync(reference);

            // Assert
            Assert.IsFalse(result); 
        }

        [Test, Order(6)]
        public async Task CancelBookingAsync_ReturnsTrue_WhenCancellationIsSuccessful()
        {
            // Arrange
            var evnt = new EventManagement { Id = 7, EventName = "Concert", Date = DateTime.Now, Venue = "Stadium", TotalSeats = 100, AvailableSeats = 50 };
            var booking = new TicketManagement { EventName = "Concert", UserName = "John", Tickets = 2, BookingReference = "ref123" };
            await _context.EventManagements.AddAsync(evnt);
            await _context.TicketManagements.AddAsync(booking);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.CancelBookingAsync(booking.BookingReference);

            // Assert
         Assert.IsFalse(_context.TicketManagements.Any(b => b.BookingReference == booking.BookingReference)); 

            Assert.IsTrue(result); 
           
        }
    }
}
