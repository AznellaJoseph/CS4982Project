using System.Text.Json.Serialization;

namespace CapstoneVendorPrototype
{
    public class Place
    {
        [JsonPropertyName("fsq_id")] public string Id { get; set; }

        public PlaceCategory[] Categories { get; set; }
        public int Distance { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public string Timezone { get; set; }
    }
}