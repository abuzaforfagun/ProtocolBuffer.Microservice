namespace Gateway
{
    public class MicroserviceConfiguration
    {
        public WeatherService WeatherService { get; set; }
    }

    public class WeatherService
    {
        public string GetAll { get; set; }
    }
}
