using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookService.Data;
using BookService.Models;

namespace BookService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingContext _context;

        public BookingController(BookingContext context)
        {
            _context = context;
        }

        // POST: api/booking
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
            booking.CreatedAt = DateTime.UtcNow;
            booking.UpdatedAt = DateTime.UtcNow;
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }

        [HttpGet]
public async Task<ActionResult<List<Booking>>> GetAllBookings()
{
    var bookings = await _context.Bookings.ToListAsync();
    return Ok(bookings); // Must return Ok(), not just bookings
}

[HttpGet("{id}")]
public async Task<ActionResult<Booking>> GetBookingById(int id)
{
    var booking = await _context.Bookings.FindAsync(id);
    if (booking == null)
    {
        return NotFound();
    }
    return Ok(booking); // Must use Ok(), not just return booking;
}
    }
}