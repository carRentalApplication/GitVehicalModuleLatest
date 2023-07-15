using CarRentalEntities;
using Microsoft.AspNetCore.Mvc;

namespace VehiclesModule.Services
{
    public interface IVehicleServices
    {
         Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle);
        Task<ActionResult<IEnumerable<Vehicle>>> GetVehicle();
        Task<ActionResult<Vehicle>> GetVehicle(int id);
        Task<IActionResult> PutVehicle(int id, Vehicle vehicle);
        Task<IActionResult> DeleteVehicle(int id);

        Task<ActionResult<int>> GetAllVehicleCount();

        Task<ActionResult<bool>> GetVehicleAvailabilityStatus(DateTime pickUpTime, DateTime dropTime,int VehicleId);

        Task<ActionResult<List<Vehicle>>> GetAvailableVehiclesBasedOnDates(DateTime FromDate,DateTime ToDate);
    

    }
}
