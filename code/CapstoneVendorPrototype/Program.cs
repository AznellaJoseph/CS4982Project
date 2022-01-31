using System;

namespace CapstoneVendorPrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            PlaceClient client = new();
            var result = client.GetPointsOfInterest(35.8308, -90.7023, 1);
            foreach (var aPlace in result.Result.Results)
            {
                Console.WriteLine(aPlace.Name);
            }
        }
    }
}
