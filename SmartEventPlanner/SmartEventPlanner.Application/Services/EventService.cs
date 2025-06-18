using SmartEventPlanner.Application.DTOs;
using SmartEventPlanner.Application.Interfaces;
using SmartEventPlanner.Domain.Entities;

namespace SmartEventPlanner.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IWeatherService _weatherService;

        public EventService(IEventRepository eventRepository, IWeatherService weatherService)
        {
            _eventRepository = eventRepository;
            _weatherService = weatherService;
        }

        public async Task<Event> CreateEventAsync(CreateEventDto dto)
        {
            var weather = await _weatherService.GetWeatherAsync(dto.Location, dto.Date);
            var weatherData = new WeatherData
            {
                Id = Guid.NewGuid(),
                Location = dto.Location,
                Date = dto.Date,
                Temperature = weather.Temperature,
                Precipitation = weather.Precipitation,
                WindSpeed = weather.WindSpeed,
                WeatherCondition = weather.WeatherCondition
            };

            var @event = new Event
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Location = dto.Location,
                Date = dto.Date,
                EventType = dto.EventType,
                WeatherData = weatherData,
                SuitabilityScore = CalculateSuitabilityScore(dto.EventType, weather)
            };

            await _eventRepository.AddEventAsync(@event);
            return @event;
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _eventRepository.GetAllEventsAsync();
        }

        public async Task<Event> UpdateEventAsync(Guid id, CreateEventDto dto)
        {
            var @event = await _eventRepository.GetEventByIdAsync(id);
            if (@event == null) throw new Exception("Event not found");

            @event.Name = dto.Name;
            @event.Location = dto.Location;
            @event.Date = dto.Date;
            @event.EventType = dto.EventType;

            var weather = await _weatherService.GetWeatherAsync(dto.Location, dto.Date);
            @event.WeatherData = new WeatherData
            {
                Id = Guid.NewGuid(),
                Location = dto.Location,
                Date = dto.Date,
                Temperature = weather.Temperature,
                Precipitation = weather.Precipitation,
                WindSpeed = weather.WindSpeed,
                WeatherCondition = weather.WeatherCondition
            };
            @event.SuitabilityScore = CalculateSuitabilityScore(dto.EventType, weather);

            await _eventRepository.UpdateEventAsync(@event);
            return @event;
        }

        public async Task<WeatherResponseDto> GetWeatherAsync(string location, DateTime date)
        {
            return await _weatherService.GetWeatherAsync(location, date);
        }

        public async Task<string> GetSuitabilityScoreAsync(Guid eventId)
        {
            var @event = await _eventRepository.GetEventByIdAsync(eventId);
            if (@event == null) throw new Exception("Event not found");
            return @event.SuitabilityScore;
        }

        public async Task<List<DateTime>> GetAlternativeDatesAsync(Guid eventId)
        {
            var @event = await _eventRepository.GetEventByIdAsync(eventId);
            if (@event == null) throw new Exception("Event not found");

            var alternatives = new List<DateTime>();
            string bestScore = @event.SuitabilityScore;

            for (int i = 1; i <= 7; i++)
            {
                var newDate = @event.Date.AddDays(i);
                try
                {
                    var weather = await _weatherService.GetWeatherAsync(@event.Location, newDate);
                    var score = CalculateSuitabilityScore(@event.EventType, weather);

                    // Add only if better than existing
                    if ((bestScore == "Poor" && (score == "Okay" || score == "Good")) ||
                        (bestScore == "Okay" && score == "Good"))
                    {
                        alternatives.Add(newDate);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return alternatives;
        }

        private string CalculateSuitabilityScore(string eventType, WeatherResponseDto weather)
        {
            int score = 0;
            string normalizedType = eventType.Trim().ToLowerInvariant();

            if (normalizedType == "outdoorsports" || normalizedType == "outdoor")
            {
                if (weather.Temperature >= 15 && weather.Temperature <= 30) score += 30;
                if (weather.Precipitation < 20) score += 25;
                if (weather.WindSpeed < 20) score += 20;
                if (weather.WeatherCondition == "Clear" || weather.WeatherCondition == "Partly cloudy") score += 25;
            }
            else if (normalizedType == "wedding")
            {
                if (weather.Temperature >= 18 && weather.Temperature <= 28) score += 30;
                if (weather.Precipitation < 10) score += 30;
                if (weather.WindSpeed < 15) score += 25;
                if (weather.WeatherCondition == "Clear" || weather.WeatherCondition == "Partly cloudy") score += 15;
            }

            return score >= 80 ? "Good" : score >= 50 ? "Okay" : "Poor";
        }
    }
}
