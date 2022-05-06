using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CapstoneBackend.Utils
{
    /// <summary>
    ///     Utility for calling the Google Places API used for Geolocation services
    /// </summary>
    public class GooglePlacesService
    {
        private const string Key = "AIzaSyB_TdvmfkvpMjDjMQnd3bDvhkNbrjRq5_I";

        /// <summary>
        ///     Autocompletes the specified search text with location results from Google.
        /// </summary>
        /// <param name="searchText">
        ///     The search text.
        /// </param>
        /// <returns>
        ///     List of autocomplete results
        /// </returns>
        public static async Task<IEnumerable<string>> Autocomplete(string searchText)
        {
            var apiEndpoint =
                $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={searchText}&types=establishment&key={Key}";
            List<string> predictions = new();
            using var client = new HttpClient();
            var response = await client.GetStringAsync(apiEndpoint);
            var result = JsonDocument.Parse(response).RootElement.GetProperty("predictions").EnumerateArray();

            while (result.MoveNext())
            {
                var currentPrediction = result.Current;
                if (!currentPrediction.TryGetProperty("description", out var description)) continue;
                var address = description.GetString();
                if (!string.IsNullOrEmpty(address)) predictions.Add(address);
            }

            return predictions;
        }


        /// <summary>
        ///     Determines whether the specified search text is a valid location according to
        ///     the Google Place Search API.
        /// </summary>
        /// <param name="searchText">
        ///     The input text.
        /// </param>
        /// <returns>
        ///     true if location is valid, false otherwise
        /// </returns>
        public static bool IsLocationValid(string searchText)
        {
            var apiEndpoint =
                $"https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input={searchText}&inputtype=textquery&key={Key}";
            var output = false;

            using var client = new HttpClient();

            try
            {
                var response = client.GetStringAsync(apiEndpoint).Result;
                var result = JsonDocument.Parse(response).RootElement.GetProperty("candidates").GetArrayLength();
                if (result > 0) output = true;
            }
            catch
            {
                output = false;
            }

            return output;
        }
    }
}