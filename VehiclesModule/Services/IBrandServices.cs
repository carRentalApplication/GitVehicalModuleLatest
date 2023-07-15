using CarRentalEntities;
using Microsoft.AspNetCore.Mvc;

namespace VehiclesModule.Services
{
    public interface IBrandServices
    {
        Task<ActionResult<Brand>> AddBrand(Brand brand);
        Task<ActionResult<IEnumerable<Brand>>> GetBrand();
        Task<ActionResult<Brand>> GetBrand(int id);
        Task<IActionResult> EditBrand(int id, Brand brand);
        Task<IActionResult> DeleteBrand(int id);
        Task<ActionResult<int>> GetAllBrandsCount();
    }
}
