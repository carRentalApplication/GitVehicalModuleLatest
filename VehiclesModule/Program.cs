using CarRentalDatabase.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Twilio.Clients;
using VehiclesModule.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IVehicleServices, VehicleServices>();
builder.Services.AddScoped<IBrandServices, BrandServices>();
builder.Services.AddScoped<IBookingServices, BookingServices>();
builder.Services.AddScoped<IPaymentTypeServices, PaymentTypeServices>();
string connectionString = builder.Configuration.GetConnectionString("MyConnectionString");
builder.Services.AddScoped<TwilioServices>();
builder.Services.AddScoped<ITwilioRestClient, TwilioClientts>();

builder.Services.AddDbContext<CarRentalDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());//for Cross-origin

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());//for Cross-origin


app.UseAuthorization();

app.MapControllers();

app.Run();
