using SmartEventPlanner.Application.DTOs;

namespace SmartEventPlanner.Application.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherResponseDto> GetWeatherAsync(string location, DateTime date);
    }
}