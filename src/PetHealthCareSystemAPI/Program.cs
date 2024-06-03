using System.Reflection;
using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using PetHealthCareSystemAPI.Middlewares;
using Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
/*_ = builder.Host.UseSerilog((httpHost, config) =>
    _ = config.ReadFrom.Configuration(builder.Configuration));*/
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ErrorHandlerMiddleware>();

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
