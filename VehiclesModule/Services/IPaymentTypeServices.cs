using CarRentalEntities;
using Microsoft.AspNetCore.Mvc;

namespace VehiclesModule.Services
{
    public interface IPaymentTypeServices
    {
        Task<ActionResult<List<PaymentType>>> GetAllPaymentTypes();
    }
}
