using CarRentalDatabase.DatabaseContext;
using CarRentalEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace VehiclesModule.Services
{
    public class VehicleServices : IVehicleServices
    {
        private readonly CarRentalDbContext _context;

        public VehicleServices(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> DeleteVehicle(int id)
        {
            if (_context.Vehicles == null)
            {
                return new NotFoundResult();
            }
            var vehicle = await _context.Vehicles.Include(e => e.VehicleImages).FirstOrDefaultAsync(e => e.VehicleId == id);
            if (vehicle == null)
            {
                return new NotFoundResult();
            }
            _context.VehicleImages.RemoveRange(vehicle.VehicleImages);
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }


        public async Task<ActionResult<int>> GetAllVehicleCount()
        {
            var vehicleCount = await _context.Vehicles.CountAsync();

            return vehicleCount;
        }




        public async Task<ActionResult<List<Vehicle>>> GetAvailableVehiclesBasedOnDates(DateTime FromDate, DateTime ToDate)
        {
            List<Vehicle> RegisteredVehicles = _context.Vehicles.Include(e=>e.VehicleImages)
                .Include(b=>b.Brand).Include(r=>r.Rent)
                .ToList();
           List<Vehicle> BookedVehicles=  _context.Bookings.Include(v=>v.Vehicle).Where(e => ((FromDate <= e.DropDate && FromDate >= e.PickUpDate) ||
           (ToDate <= e.DropDate && ToDate >= e.PickUpDate))).Select(v=>v.Vehicle).ToList();
            List<Vehicle> AvailableVehicles= RegisteredVehicles.Except(BookedVehicles).ToList();
            return AvailableVehicles;
        }


        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicle()
        {
            if (_context.Vehicles == null)

            {
                return new  NotFoundResult();
            }
            return await _context.Vehicles.Include(x => x.Brand).Include(x => x.Rent).Include(x => x.VehicleImages).ToListAsync();
        }

        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            if (_context.Vehicles == null)
            {
                return new NotFoundResult();
            }
            var vehicle = await _context.Vehicles.Include(x => x.Brand).Include(x => x.Rent).Include(x => x.VehicleImages).FirstOrDefaultAsync(x => x.VehicleId == id);

            if (vehicle == null)
            {
                return new NotFoundResult();
            }

            return vehicle;
        }

        public async Task<ActionResult<bool>> GetVehicleAvailabilityStatus(DateTime pickUpTime,DateTime dropTime, int VehicleId)
        {
            
           
           Booking booking= await _context.Bookings.FirstOrDefaultAsync(e => ((pickUpTime <= e.DropDate && pickUpTime >= e.PickUpDate) ||
           (dropTime <= e.DropDate && dropTime >= e.PickUpDate)) && e.Vehicle.VehicleId==VehicleId);
            return booking == null;
        }

        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return new BadRequestResult();
            }

            // Retrieve the existing brand from the database
            var existingBrand = _context.Brands.FirstOrDefault(e => e.BrandName.Equals(vehicle.Brand.BrandName));

            if (existingBrand == null)
            {
                _context.Brands.Add(vehicle.Brand);
                await _context.SaveChangesAsync();
                existingBrand = _context.Brands.FirstOrDefault(e => e.BrandName.Equals(vehicle.Brand.BrandName));
            }

            // Assign the existing brand to the vehicle
            vehicle.Brand = existingBrand;

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return new  CreatedAtActionResult("GetVehicle","Vehicles", new { id = vehicle.VehicleId }, vehicle);
        }

        public async Task<IActionResult> PutVehicle(int id, Vehicle vehicle)
        {
            if (id != vehicle.VehicleId)
            {
                return new BadRequestResult();
            }

            _context.Entry(vehicle).State = EntityState.Modified;
            _context.Entry(vehicle.Rent).State = EntityState.Modified;
            _context.Entry(vehicle.Brand).State = EntityState.Modified;
            foreach (var Image in vehicle.VehicleImages)
            {
                _context.Entry(Image).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
                {
                    return new  NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }

        private bool VehicleExists(int id)
        {
            return (_context.Vehicles?.Any(e => e.VehicleId == id)).GetValueOrDefault();
        }
    }
}
