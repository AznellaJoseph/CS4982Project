using System.Text.Json.Serialization;

namespace CapstoneVendorPrototype
{
    /// <summary>
    ///     Place Class
    /// </summary>
    public class Place
    {
        /// <summary>
        ///     The id.
        /// </summary>
        [JsonPropertyName("fsq_id")]
        public string Id { get; set; }

        /// <summary>
        ///     The categories.
        /// </summary>
        public PlaceCategory[] Categories { get; set; }

        /// <summary>
        ///     The distance.
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        ///     The location.
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        ///     The name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The timezone.
        /// </summary>
        public string Timezone { get; set; }
    }
}