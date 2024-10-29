using Microsoft.EntityFrameworkCore;
using Ticket_Booking_App.Models;
using Ticket_Booking_App.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EventManagementBDContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("emtcs")));
//builder.Services.AddScoped<IEventManagementService, EventManagementService>();
builder.Services.AddScoped<EventManagementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseCors(options =>
//options.WithOrigins("http://localhost:4200/")
//.AllowAnyMethod()
//.AllowAnyHeader()
//);

app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();

app.MapControllers();

app.Run();
