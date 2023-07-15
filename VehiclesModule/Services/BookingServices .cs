using CarRentalDatabase.DatabaseContext;
using CarRentalEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace VehiclesModule.Services
{
    public class BookingServices : IBookingServices
    {
        private readonly CarRentalDbContext _context;


        public BookingServices(CarRentalDbContext context)
        {
            _context = context;

        }

        /*public async Task<ActionResult<Booking>> AddBooking(Booking booking)
        {
            // Perform any necessary validation or business logic before adding the booking

            booking.TotalAmount = CalculateTotalAmount(booking.PickUpDate, booking.DropDate);

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult("GetBooking", "Booking", new { id = booking.BookingId }, booking);
        }*/

        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBooking()
        {
            var bookings = await _context.Bookings.Include(s=>s.Status).Include(v=>v.Vehicle)
                .Include(p=>p.PaymentType).Include(b=>b.Vehicle.Brand).Include(r=>r.Vehicle.Rent).ToListAsync();
            return bookings;
        }

        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return new NotFoundResult();
            }

            return booking;
        }

        public async Task<ActionResult<Booking>> UpdateBooking(int id,string status) //accept
        {
            var bookingDeatils = await _context.Bookings.FindAsync(id);

            var statusDetails = await _context.Statuses.FirstOrDefaultAsync(e=>e.StatusName==status);
            if(statusDetails==null)
            {
                Status status1 = new Status();
                status1.StatusName=status;
                await _context.Statuses.AddAsync(status1);
                await _context.SaveChangesAsync();
                statusDetails = await _context.Statuses.FirstOrDefaultAsync(e => e.StatusName == status);
            }

            if (bookingDeatils == null)
            {
                return new NotFoundResult();
            }

            // Update the properties of the booking

            bookingDeatils.Status = statusDetails;

            await _context.SaveChangesAsync();
            // Calculate the total amount based on pick-up and drop-off dates
            
            return bookingDeatils;
        }

        public async Task<ActionResult<Booking>> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return new NotFoundResult();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return booking;
        }
        public async Task<ActionResult<List<Booking>>> GetBookingsByUserId(string strUserId)
        {
            Guid UserId = new Guid(strUserId);
            var booking = await _context.Bookings
                .Include(s => s.Status)
                .Include(p => p.PaymentType)
                .Include(u => u.AuthUser)
                .Include(v=>v.Vehicle).Where(b => b.AuthUser.UserId == UserId).ToListAsync();

            return booking;

        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }

        private double CalculateTotalAmount(DateTime pickUpDate, DateTime dropDate)
        {
            // Calculate the total amount based on pick-up date and drop-off date
            // Implement your logic here to calculate the total amount
            // You can use the pickUpDate and dropDate parameters to calculate the duration of the booking

            // Example calculation: total amount = duration in days * daily rental rate
            double durationInHours = (dropDate - pickUpDate).TotalHours;
            double hourlyRentalRate = 10.00; // Example hourly rental rate
            double totalAmount = durationInHours * hourlyRentalRate;

            return totalAmount;
        }

        public async Task<ActionResult<Booking>> AddBooking(BookingModel model)
        {
            Guid UserId = new Guid(model.userId);
            AuthUser authUser= await _context.AuthUsers.FirstOrDefaultAsync(u=>u.UserId==UserId);
            Vehicle vehicle = await _context.Vehicles.FindAsync(model.VehicleId);
            Status status = await _context.Statuses.FirstOrDefaultAsync(e => e.StatusName == "Pending");
            if(status == null) {
                Status s = new Status();
                s.StatusName = "Pending";
                await _context.Statuses.AddAsync(s);
                await _context.SaveChangesAsync(); 
                status = s;
                //status = await _context.Statuses.FirstOrDefaultAsync(e => e.StatusName == "Pending");
            }
            PaymentType paymentType = await _context.PaymentTypes.FirstOrDefaultAsync(p => p.PaymentMode == model.PaymentMode);
            Booking booking=GetBookingFromBookingModel(authUser, status, vehicle,paymentType,model);
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult("GetBooking", "Booking", new { id = booking.BookingId }, booking);

        }
        private  Booking GetBookingFromBookingModel(AuthUser authUser, Status status,Vehicle vehicle, PaymentType paymentType, BookingModel model)
        {
            Booking booking = new Booking();
            booking.TravellerName = model.TravallerName;
            booking.TravellerNumber = model.TravallerNumber;
            string format = "yyyy-MM-ddTHH:mm:ss.fffZ";
            DateTime fromDate = DateTime.ParseExact(model.PickUpDate,format ,
                                       System.Globalization.CultureInfo.InvariantCulture);

            DateTime toDate = DateTime.ParseExact(model.DropDate, format,
                                       System.Globalization.CultureInfo.InvariantCulture);
            booking.PickUpDate=fromDate;
            booking.DropDate = toDate;
            booking.BookingTime = new DateTime();
            booking.PickUpAddress = model.PickUpAddress;
            booking.TotalAmount = model.TotalAmount;
           
           booking.Status = status;
            booking.AuthUser = authUser;
            booking.Vehicle=vehicle;
            booking.PaymentType=paymentType;
            booking.AdvanceAmount = 0;
            return booking;
        }
    }
}