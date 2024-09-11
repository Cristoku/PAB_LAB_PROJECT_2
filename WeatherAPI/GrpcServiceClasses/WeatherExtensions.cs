using Grpc.Net.Client;
using WeatherApi;
using WeatherAPI.Domain;

namespace WeatherAPI.GrpcServiceClasses
{
    public static class WeatherExtensions
    {
        public static void NameWeather(this Weather weather)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7241");
            var client = new WeatherNaming.WeatherNamingClient(channel);
            var request = client.DescribeWeather(new WeatherRequest { Temperature = weather.Temperature });
            weather.Feeling = request.Message;
        }
    }
}
