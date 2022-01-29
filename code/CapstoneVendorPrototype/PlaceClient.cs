using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace CapstoneVendorPrototype
{
    /// <summary>
    /// A Client to Connect to the FourSquare Places API
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class PlaceClient : IDisposable
    {

        private readonly RestClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceClient"/> class.
        /// </summary>
        public PlaceClient()
        {
            var options = new RestClientOptions("https://api.foursquare.com/v3/places");
            this._client = new RestClient(options)
                .AddDefaultHeaders(new Dictionary<string, string>() {
                    {"Accept", "application/json"}, 
                    {"Authorization", "fsq3b6JgILotAnLzn8obbDJ807YsnxchwjdXlo1mNXffycY="}
                });
        }

        /// <summary>
        /// Gets the points of interest.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>the a json string of points of intrests</returns>
        public async Task<string> GetPointsOfInterest(double latitude, double longitude)
        {
            var request = new RestRequest("nearby")
               .AddQueryParameter("ll", $"{latitude},{longitude}");
                
            var response = await this._client.ExecuteAsync(request);
            return response!.Content;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
