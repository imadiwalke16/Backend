
namespace SmartEventPlanner.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string EventType { get; set; } = string.Empty; // e.g., OutdoorSports, Wedding
        public WeatherData? WeatherData { get; set; }
        public string SuitabilityScore { get; set; } = string.Empty; // Good, Okay, Poor
    }
}