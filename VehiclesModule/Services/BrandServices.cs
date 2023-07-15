using CarRentalDatabase.DatabaseContext;
using CarRentalEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VehiclesModule.Services
{
    public class BrandServices : IBrandServices
    {
        private readonly CarRentalDbContext _context;
        private readonly IVehicleServices vehicleServices;
        public BrandServices(CarRentalDbContext context, IVehicleServices vehicleServices)
        {
            _context = context;
            this.vehicleServices = vehicleServices;
        }

        public async Task<ActionResult<Brand>> AddBrand(Brand brand)
        {
            if (_context.Brands == null)
            {
                return new BadRequestResult();
            }
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult("GetBrand","Brand", new { id = brand.BrandId }, brand);
        }

        public async Task<IActionResult> DeleteBrand(int id)
        {
            if (_context.Brands == null)
            {
                return new NotFoundResult();
            }
            var brand = await _context.Brands.Include(e => e.Vehicles).FirstOrDefaultAsync(e => e.BrandId == id);
            if (brand == null)
            {
                return new NotFoundResult();
            }
            var vehicleIdsToDelete = brand.Vehicles.Select(v => v.VehicleId).ToList();

            foreach (var vehicleId in vehicleIdsToDelete)
            {
                await vehicleServices.DeleteVehicle(vehicleId);
            }
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<IActionResult> EditBrand(int id, Brand brand)
        {
            if (id != brand.BrandId)
            {
                return new BadRequestResult();
            }

            _context.Entry(brand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }

        public async Task<ActionResult<int>> GetAllBrandsCount()
        {
            var brandCount = await _context.Brands.CountAsync();

            return brandCount;
        }

        public async Task<ActionResult<IEnumerable<Brand>>> GetBrand()
        {
            if (_context.Brands == null)
            {
                return new NotFoundResult();
            }
            return await _context.Brands.ToListAsync();
        }

        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            if (_context.Brands == null)
            {
                return new NotFoundResult();
            }
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
            {
                return new NotFoundResult();
            }

            return brand;
        }
        private bool BrandExists(int id)
        {
            return (_context.Brands?.Any(e => e.BrandId == id)).GetValueOrDefault();
        }
    }
}

