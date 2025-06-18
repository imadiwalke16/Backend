using Microsoft.AspNetCore.Mvc;
using SmartEventPlanner.Application.DTOs;
using SmartEventPlanner.Application.Interfaces;
using SmartEventPlanner.Domain.Entities;

namespace SmartEventPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent([FromBody] CreateEventDto dto)
        {
            try
            {
                var @event = await _eventService.CreateEventAsync(dto);
                return CreatedAtAction(nameof(GetAllEvents), new { id = @event.Id }, @event);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> GetAllEvents()
        {
            try
            {
                var events = await _eventService.GetAllEventsAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Event>> UpdateEvent(Guid id, [FromBody] CreateEventDto dto)
        {
            try
            {
                var @event = await _eventService.UpdateEventAsync(id, dto);
                return Ok(@event);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("weather/{location}/{date}")]
        public async Task<ActionResult<WeatherResponseDto>> GetWeather(string location, DateTime date)
        {
            try
            {
                var weather = await _eventService.GetWeatherAsync(location, date);
                return Ok(weather);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("{id}/weather-check")]
        public async Task<ActionResult<string>> CheckWeatherSuitability(Guid id)
        {
            try
            {
                var score = await _eventService.GetSuitabilityScoreAsync(id);
                return Ok(score);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/suitability")]
        public async Task<ActionResult<string>> GetSuitability(Guid id)
        {
            try
            {
                var score = await _eventService.GetSuitabilityScoreAsync(id);
                return Ok(score);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/alternatives")]
        public async Task<ActionResult<List<DateTime>>> GetAlternativeDates(Guid id)
        {
            try
            {
                var dates = await _eventService.GetAlternativeDatesAsync(id);
                return Ok(dates);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}