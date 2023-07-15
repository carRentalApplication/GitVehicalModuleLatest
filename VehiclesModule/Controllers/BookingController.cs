using CarRentalEntities;
using Microsoft.AspNetCore.Mvc;
using VehiclesModule.Services;

namespace VehiclesModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingServices _bookingServices;

        public BookingController(IBookingServices bookingServices)
        {
            _bookingServices = bookingServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
        {
            var bookings = await _bookingServices.GetAllBooking();
            return bookings;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _bookingServices.GetBooking(id);
            if (booking.Value == null)
            {
                return NotFound();
            }

            return booking.Value;
        }

        [HttpGet("user/{strUserId}")]
        public async Task<ActionResult<List<Booking>>> GetAllBookingsByUserId(string strUserId)
        {
            return await _bookingServices.GetBookingsByUserId(strUserId);
        }

        /*[HttpPost]
        public async Task<ActionResult<Booking>> AddBooking(Booking booking)
        {
            var result = await _bookingServices.AddBooking(booking);
            return result.Result;
        }*/
        [HttpPost]
        public async Task<ActionResult<Booking>> AddBooking(BookingModel model)
        {
            /*var result = await _bookingServices.AddBooking(booking);
            return result.Result;*/

            return await _bookingServices.AddBooking(model);
        }

        [HttpPut("{id}/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Booking>> UpdateBooking(int id,string status)
        {
            
            if (id == null)
            {
                return BadRequest();
            }

            var result = await _bookingServices.UpdateBooking(id, status);
            if (result.Value == null)
            {
                return NotFound();
            }

            return result.Value;
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Booking>> DeleteBooking(int id)
        {
            var result = await _bookingServices.DeleteBooking(id);
            if (result.Value == null)
            {
                return NotFound();
            }

            return result.Value;
        }
    }
}