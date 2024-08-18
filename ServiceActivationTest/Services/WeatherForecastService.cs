using Microsoft.Extensions.Logging;
using ServiceActivationTest.Controllers;

namespace ServiceActivationTest.Services
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> GetForecasts();
        string GetWeatherForecastServiceId();
    }

    public class WeatherForecastService : IWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastService> _logger;
        private readonly Guid _id;

        public WeatherForecastService(ILogger<WeatherForecastService> logger)
        {
            _logger = logger;
            _id = Guid.NewGuid();
        }

        public string GetWeatherForecastServiceId()
        {
            return _id.ToString();
        }

        public IEnumerable<WeatherForecast> GetForecasts()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                ServiceId = _id,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
