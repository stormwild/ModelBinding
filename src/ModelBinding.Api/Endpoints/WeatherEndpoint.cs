using System.Security.Cryptography;

namespace ModelBinding.Api.Endpoints;

public class WeatherEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        app.MapGet("/weatherforecast", () =>
        {

            var _ = Random.Shared.Next();
            var rng = RandomNumberGenerator.Create();

            var temperatureBytes = new byte[4];
            rng.GetBytes(temperatureBytes);
            // generate a random temperature between -20 and 55
            var temperatureC = BitConverter.ToInt32(temperatureBytes, 0) % 75 - 20;

            var summaryIndexBytes = new byte[4];
            rng.GetBytes(summaryIndexBytes);
            var summaryIndex = BitConverter.ToInt32(summaryIndexBytes, 0) % summaries.Length;


            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                 DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    temperatureC,
                    summaries[summaryIndex]
                ))
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast");
    }
}

sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
