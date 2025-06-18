using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SmartEventPlanner.Application.DTOs;
using SmartEventPlanner.Application.Interfaces;
using System.Net.Http;
using System.Text.Json;

namespace SmartEventPlanner.Infrastructure.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IMemoryCache _cache;

        public WeatherService(HttpClient httpClient, IConfiguration configuration, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeatherMap:ApiKey"];
            _cache = cache;
        }

        public async Task<WeatherResponseDto> GetWeatherAsync(string location, DateTime date)
        {
            string cacheKey = $"{location}_{date:yyyyMMdd}";
            if (_cache.TryGetValue(cacheKey, out WeatherResponseDto? cached))
            {
                return cached!;
            }

            // Step 1: Get coordinates from geo API
            string geoUrl = $"http://api.openweathermap.org/geo/1.0/direct?q={location}&limit=1&appid={_apiKey}";
            var geoResponse = await _httpClient.GetAsync(geoUrl);
            if (!geoResponse.IsSuccessStatusCode)
            {
                throw new Exception("Invalid location or Geo API failure");
            }

            var geoJson = await geoResponse.Content.ReadAsStringAsync();
            var geoData = JsonDocument.Parse(geoJson).RootElement;
            if (geoData.GetArrayLength() == 0)
            {
                throw new Exception("Location not found in Geo API");
            }
            double lat = geoData[0].GetProperty("lat").GetDouble();
            double lon = geoData[0].GetProperty("lon").GetDouble();

            // Step 2: Fetch forecast data
            string forecastUrl = $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={_apiKey}&units=metric";
            var forecastResponse = await _httpClient.GetAsync(forecastUrl);
            if (!forecastResponse.IsSuccessStatusCode)
            {
                throw new Exception("Forecast API failed");
            }

            var forecastJson = await forecastResponse.Content.ReadAsStringAsync();
            var forecastRoot = JsonDocument.Parse(forecastJson).RootElement;
            var forecastList = forecastRoot.GetProperty("list");

            // Step 3: Find closest forecast entry to requested date
            DateTime targetDate = date.Date;
            WeatherResponseDto? bestMatch = null;
            double smallestDiff = double.MaxValue;

            foreach (var entry in forecastList.EnumerateArray())
            {
                DateTime entryDateTime = DateTime.Parse(entry.GetProperty("dt_txt").GetString()!);
                if (entryDateTime.Date != targetDate) continue;

                double diff = Math.Abs((entryDateTime - date).TotalHours);
                if (diff < smallestDiff)
                {
                    smallestDiff = diff;
                    var main = entry.GetProperty("main");
                    var weather = entry.GetProperty("weather")[0];
                    var wind = entry.GetProperty("wind");

                    bestMatch = new WeatherResponseDto
                    {
                        Temperature = main.GetProperty("temp").GetDouble(),
                        Precipitation = entry.TryGetProperty("rain", out var rain) && rain.TryGetProperty("3h", out var r3h) ? r3h.GetDouble() : 0,
                        WindSpeed = wind.GetProperty("speed").GetDouble() * 3.6, // m/s to km/h
                        WeatherCondition = weather.GetProperty("main").GetString() ?? "Unknown"
                    };
                }
            }

            if (bestMatch == null)
            {
                throw new Exception("No weather data available for the given date");
            }

            _cache.Set(cacheKey, bestMatch, TimeSpan.FromHours(3));
            return bestMatch;
        }
    }
}
