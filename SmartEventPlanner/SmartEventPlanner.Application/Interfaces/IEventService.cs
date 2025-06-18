using SmartEventPlanner.Application.DTOs;
using SmartEventPlanner.Domain.Entities;

namespace SmartEventPlanner.Application.Interfaces
{
    public interface IEventService
    {
        Task<Event> CreateEventAsync(CreateEventDto dto);
        Task<List<Event>> GetAllEventsAsync();
        Task<Event> UpdateEventAsync(Guid id, CreateEventDto dto);
        Task<WeatherResponseDto> GetWeatherAsync(string location, DateTime date);
        Task<string> GetSuitabilityScoreAsync(Guid eventId);
        Task<List<DateTime>> GetAlternativeDatesAsync(Guid eventId);
    }
}