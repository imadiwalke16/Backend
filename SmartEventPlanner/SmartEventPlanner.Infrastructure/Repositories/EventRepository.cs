using Microsoft.EntityFrameworkCore;
using SmartEventPlanner.Application.Interfaces;
using SmartEventPlanner.Domain.Entities;
using SmartEventPlanner.Infrastructure.Data;

namespace SmartEventPlanner.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddEventAsync(Event @event)
        {
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Include(e => e.WeatherData)
                .ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(Guid id)
        {
            return await _context.Events
                .Include(e => e.WeatherData)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateEventAsync(Event @event)
        {
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
        }
    }
}