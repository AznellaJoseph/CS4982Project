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
        private const string Key = "AIzaSyDmYx_C23N0TLFO234gBQBBL3EMZ9HYIG4";

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
                if (currentPrediction.TryGetProperty("description", out var description))
                    predictions.Add(description.GetString() ?? string.Empty);
            }

            return predictions;
        }
    }
}