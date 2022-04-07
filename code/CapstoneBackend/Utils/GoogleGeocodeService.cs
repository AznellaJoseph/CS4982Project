using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CapstoneBackend.Utils
{
    public class GoogleGeocodeService
    {

        private const string key = "AIzaSyDmYx_C23N0TLFO234gBQBBL3EMZ9HYIG4";

        public struct Location
        {
            public double Latitude;
            public double Longitude;

        }
        public static Location GetLocationByAddress(string address)
        {
            string addressWithNoSpaces = address.Replace(" ", "+");
            var apiEndpoint = $"https://maps.googleapis.com/maps/api/geocode/json?address={addressWithNoSpaces}&key={key}";
            Location location = new();
            string response;
            using (var client = new HttpClient())
            { 
                response = client.GetStringAsync(apiEndpoint).Result;
            }
            var result = JsonDocument.Parse(response).RootElement.GetProperty("results")[0].GetProperty("geometry")
                                     .GetProperty("location");
            location.Latitude = result.GetProperty("lat").GetDouble();
            location.Longitude = result.GetProperty("lng").GetDouble();
            return location;
        }
    }
}
