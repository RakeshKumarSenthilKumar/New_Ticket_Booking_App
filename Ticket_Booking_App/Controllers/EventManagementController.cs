using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ticket_Booking_App.Models;
using Ticket_Booking_App.Services;

namespace Ticket_Booking_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventManagementController : ControllerBase
    {
        private readonly EventManagementService _service;

        public EventManagementController(EventManagementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventManagement>>> GetEvents()
        {
            return Ok(await _service.GetEventsAsync());
        }

        [HttpPost("event")]
        public async Task<ActionResult> CreateEvent(EventManagement eventManagement)
        {
            await _service.CreateEventAsync(eventManagement);
            return Ok("An event is added");
        }

        [HttpGet("AllTickets")]
        public async Task<ActionResult<IEnumerable<TicketManagement>>> GetTickets()
        {
            return Ok(await _service.GetTicketsAsync());
        }

        [HttpPost("book")]
        public async Task<ActionResult> BookTickets(TicketBookingRequest request)
        {
            var booking = await _service.BookTicketsAsync(request);
            return booking == null ? BadRequest("Not enough seats available.") : Ok(booking);
        }

        [HttpDelete("cancel")]
        public async Task<ActionResult> CancelBooking(TicketCancel tc)
        {
            var success = await _service.CancelBookingAsync(tc.Referrence);
            return success ? Ok(new { message = "Booking canceled successfully." }) : NotFound("Booking not found.");
        }
    }
}
