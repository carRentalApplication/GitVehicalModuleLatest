using CarRentalEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRentalDatabase;
using CarRentalDatabase.DatabaseContext;
using VehiclesModule.Services;

namespace VehiclesModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleServices _vehicleServices;

        public VehiclesController(IVehicleServices vehicleServices)
        {
            _vehicleServices = vehicleServices;
        }

        // GET: api/Vehicles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicle()
        {
            return await _vehicleServices.GetVehicle();

        }

        // GET: api/Vehicles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            return await _vehicleServices.GetVehicle(id);
        }
        [HttpGet("register-vehicle-count")]
        public async Task<ActionResult<int>> GetAllVehicleCount()
        {
            return await _vehicleServices.GetAllVehicleCount();
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(int id, Vehicle vehicle)
        {
            return await _vehicleServices.PutVehicle(id, vehicle);
        }


        [HttpPost]
        // [Produces("application/json", "application/xml")]
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {

          
           
           
            return await _vehicleServices.PostVehicle(vehicle);

        }


        // DELETE: api/Vehicles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            return await _vehicleServices.DeleteVehicle(id);

        }

        [HttpGet("status")]
        public async Task<ActionResult<bool>> GetVehicleAvailabilityStatus(DateTime pickUpTime, DateTime dropTime, int VehicleId)
        {
            return await _vehicleServices.GetVehicleAvailabilityStatus(pickUpTime, dropTime, VehicleId);
        }
        [HttpGet("availablevehicles")]
        public async Task<ActionResult<List<Vehicle>>> GetAvailableVehiclesBasedOnDate(DateTime fromDate,DateTime toDate)
        {
            return await _vehicleServices.GetAvailableVehiclesBasedOnDates(fromDate, toDate);
        }

    }
}
