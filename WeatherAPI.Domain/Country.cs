﻿namespace WeatherAPI.Domain
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Region> Regions { get; set; }
    }
}
