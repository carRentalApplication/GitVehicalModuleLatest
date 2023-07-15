using CarRentalDatabase.DatabaseContext;
using CarRentalEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VehiclesModule.Services
{
    public class PaymentTypeServices : IPaymentTypeServices
    {
        private readonly CarRentalDbContext dbContext;
        public PaymentTypeServices(CarRentalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ActionResult<List<PaymentType>>> GetAllPaymentTypes()
        {
            return await dbContext.PaymentTypes.ToListAsync();
        }
    }
}
