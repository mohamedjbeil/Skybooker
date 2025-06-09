using Microsoft.EntityFrameworkCore;
using BookService.Data;
using Microsoft.OpenApi.Models; // For Swagger

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookingContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<BookService.Data.BookingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();