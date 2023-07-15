using CarRentalEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehiclesModule.Services;

namespace VehiclesModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandServices brandServices;
        public BrandController(IBrandServices brandServices)
        {
            this.brandServices = brandServices;
        }
        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrand()
        {
            return await brandServices.GetBrand();
        }

        // GET: api/Brands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            return await brandServices.GetBrand(id);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(int id, Brand brand)
        {
            return await brandServices.EditBrand(id, brand);
        }
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            return await brandServices.AddBrand(brand);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            return await brandServices.DeleteBrand(id);
        }
        [HttpGet("register-brand-count")]
        public async Task<ActionResult<int>> GetAllBrandsCount()
        {
            return await brandServices.GetAllBrandsCount();
        }
    }
}
