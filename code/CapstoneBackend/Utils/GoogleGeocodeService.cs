using System.Net.Http;
using System.Text.Json;

namespace CapstoneBackend.Utils
{
    /// <summary>
    /// Utility for calling the Google Geocode API for retrieving longitude and latitude for maps
    /// </summary>
    public class GoogleGeocodeService
    {
        private const string Key = "AIzaSyB_TdvmfkvpMjDjMQnd3bDvhkNbrjRq5_I";

        /// <summary>
        /// Gets the location by address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>A location object with the longitude and latitude of the entered address</returns>
        public static Location GetLocationByAddress(string address)
        {
            var addressWithNoSpaces = address.Replace(" ", "+");
            var apiEndpoint =
                $"https://maps.googleapis.com/maps/api/geocode/json?address={addressWithNoSpaces}&key={Key}";
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

        /// <summary>
        /// Location object used to store longitudes and latitudes
        /// </summary>
        public struct Location
        {
            public double Latitude;
            public double Longitude;
        }
    }
}