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
                    var currPrediction = result.Current;
                    if (currPrediction.TryGetProperty("description", out JsonElement description))
                    {
                        var address = description.GetString();
                        if (!string.IsNullOrEmpty(address))
                        {
                            predictions.Add(address);
                        }
                    }
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
        ///   <c>true</c> if location is valid, otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLocationValid(string searchText)
        {
            var apiEndpoint = $"https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input={searchText}&inputtype=textquery&key={Key}";
            bool output = false;

            using (var client = new HttpClient())
            {
                string response;

                try
                {
                    response = client.GetStringAsync(apiEndpoint).Result;
                    int result = JsonDocument.Parse(response).RootElement.GetProperty("candidates").GetArrayLength();
                    if (result > 0)
                    {
                        output = true;
                    }
                }
                catch
                {
                    output = false;
                }
            }

            return output;
        }
    }
}
