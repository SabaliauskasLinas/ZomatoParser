﻿using Weather.Core.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Core.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly string _host = "https://api.weatherbit.io/v2.0/current";
        private readonly string _key = "0bd4d9bbcce34b7b8c4fbf76bdc4fdf0";

        public ServerResult<WeatherDetails> GetWeather(WeatherArgs args)
        {
            try
            {
                var url = $"{_host}?" +
                    $"&key={_key}" +
                    $"&lat={args.Latitude.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}" +
                    $"&lon={args.Longitude.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}";

                var client = new RestClient(url)
                {
                    Timeout = -1
                };

                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);

                if (!response.IsSuccessful)
                    return new ServerResult<WeatherDetails>() { Success = false, Message = response.ErrorMessage };

                var content = JObject.Parse(response.Content);
                JToken result = content["data"].Children().FirstOrDefault();

                return new ServerResult<WeatherDetails>() { Success = true, Data = result.ToObject<WeatherDetails>() };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ServerResult<WeatherDetails>()
                {
                    Success = false,
                    Message = "Error while trying to fetch data",
                };
            }
        }
    }
}
