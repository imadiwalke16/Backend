using Microsoft.EntityFrameworkCore;
using SmartEventPlanner.Application.Interfaces;
using SmartEventPlanner.Application.Services;
using SmartEventPlanner.Infrastructure.Data;
using SmartEventPlanner.Infrastructure.Repositories;
using SmartEventPlanner.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Urls.Add("http://0.0.0.0:3000");

app.Run();
