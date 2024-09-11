using Grpc.Core;

namespace WeatherNamingService.Services
{
    public class WeatherNamingService : WeatherNaming.WeatherNamingBase
    {
        public override Task<WeatherReply> DescribeWeather(WeatherRequest request, ServerCallContext context)
        {
            string message;

            if (request.Temperature < 0)
                message = "Mro�no";
            else if (request.Temperature < 10)
                message = "Bardzo zimno";
            else if (request.Temperature < 20)
                message = "Zimno";
            else if (request.Temperature < 30)
                message = "Ciep�o";
            else if (request.Temperature < 40)
                message = "Gor�co";
            else
                message = "Bardzo gor�co";

            return Task.FromResult(new WeatherReply
            {
                Message = message
            });
        }
    }
}
