using CarRentalEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehiclesModule.Services;

namespace VehiclesModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTypeController : ControllerBase
    {
        private readonly IPaymentTypeServices paymentTypeServices;
        public PaymentTypeController(IPaymentTypeServices paymentTypeServices)
        {
            this.paymentTypeServices = paymentTypeServices;
        }
        [HttpGet]
        public async Task<ActionResult<List<PaymentType>>> GetAllPaymentTypes()
        {
            return await paymentTypeServices.GetAllPaymentTypes();
        }
    }
}
