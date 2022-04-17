using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CapstoneBackend.Utils
{
    /// <summary>
    ///   <para>Utility for calling the Google Places API</para>
    ///   <para>Used for Geolocation services.</para>
    /// </summary>
    public class GooglePlacesService
    {
        private const string key = "AIzaSyDmYx_C23N0TLFO234gBQBBL3EMZ9HYIG4";

        /// <summary>
        ///     Autocompletes the specified search text with location results from Google.
        /// </summary>
        /// <param name="searchText">
        ///     The search text.
        /// </param>
        /// <returns>
        ///     List of autocomplete results
        /// </returns>
        public static async Task<IEnumerable<string>> Autocomplete (string searchText)
        {
            var apiEndpoint = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={searchText}&types=establishment&key={key}";
            List<string> predictions = new();
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(apiEndpoint);
                var result = JsonDocument.Parse(response).RootElement.GetProperty("predictions").EnumerateArray();

                while (result.MoveNext())
                {
                    var currPrediction = result.Current;
                    if (currPrediction.TryGetProperty("description", out JsonElement description))
                    {
                        predictions.Add(description.GetString());
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
        public static async Task<bool> IsLocationValid(string searchText)
        {
            var apiEndpoint = $"https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input={searchText}&inputtype=textquery&key={key}";
            bool output = false;

            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(apiEndpoint);
                int result = JsonDocument.Parse(response).RootElement.GetProperty("candidates").GetArrayLength();
                if (result > 0)
                {
                    output = true;
                }
            }

            return output;
        }
    }
}
