namespace SmartEventPlanner.Application.DTOs
{
    public class CreateEventDto
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string EventType { get; set; } = string.Empty;
    }
}