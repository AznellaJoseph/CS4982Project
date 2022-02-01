using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace CapstoneVendorPrototype
{
    /// <summary>
    ///     A Client to Connect to the FourSquare Places API
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class PlaceClient : IDisposable
    {
        private readonly RestClient _client;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlaceClient" /> class.
        /// </summary>
        public PlaceClient()
        {
            var options = new RestClientOptions("https://api.foursquare.com/v3/places");
            _client = new RestClient(options)
                .AddDefaultHeaders(new Dictionary<string, string>
                {
                    {"Accept", "application/json"},
                    {"Authorization", "fsq3b6JgILotAnLzn8obbDJ807YsnxchwjdXlo1mNXffycY="}
                });
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Gets the points of interest.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="radius">The radius</param>
        /// <returns>the a json string of points of interests</returns>
        public async Task<PlacesResult> GetPointsOfInterest(double latitude, double longitude, int radius)
        {
            var request = new RestRequest("nearby")
                .AddQueryParameter("ll", $"{latitude},{longitude}")
                .AddQueryParameter("radius", $"{radius}");
            return await _client.GetAsync<PlacesResult>(request);
        }
    }
}