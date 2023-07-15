using CarRentalEntities;
using Microsoft.AspNetCore.Mvc;

namespace VehiclesModule.Services
{
    public interface IBookingServices
    {
        // Task<ActionResult<Booking>> AddBooking(Booking booking);
        Task<ActionResult<Booking>> AddBooking(BookingModel model);
        Task<ActionResult<IEnumerable<Booking>>> GetAllBooking();
        Task<ActionResult<Booking>> GetBooking(int id);

        Task<ActionResult<Booking>> UpdateBooking(int id, string status);
        Task<ActionResult<Booking>> DeleteBooking(int id);
        Task<ActionResult<List<Booking>>> GetBookingsByUserId(string strUserId);

    }
}