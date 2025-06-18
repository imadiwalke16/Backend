namespace SmartEventPlanner.Application.DTOs
{
    public class WeatherResponseDto
    {
        public double Temperature { get; set; }
        public double Precipitation { get; set; }
        public double WindSpeed { get; set; }
        public string WeatherCondition { get; set; } = string.Empty;
    }
}