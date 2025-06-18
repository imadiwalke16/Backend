using SmartEventPlanner.Domain.Entities;

namespace SmartEventPlanner.Application.Interfaces
{
    public interface IEventRepository
    {
        Task AddEventAsync(Event @event);
        Task<List<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(Guid id);
        Task UpdateEventAsync(Event @event);
    }
}