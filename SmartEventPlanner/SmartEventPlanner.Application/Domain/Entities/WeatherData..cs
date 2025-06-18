namespace SmartEventPlanner.Domain.Entities
{
    public class WeatherData
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public double Temperature { get; set; } // Celsius
        public double Precipitation { get; set; } // Percentage
        public double WindSpeed { get; set; } // km/h
        public string WeatherCondition { get; set; } = string.Empty; // e.g., Clear, Cloudy
    }
}