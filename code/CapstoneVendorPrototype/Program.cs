using System;

namespace CapstoneVendorPrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            PlaceClient client = new();
            var result = client.GetPointsOfInterest(35.8308, -90.7023);
            Console.WriteLine(result.Result);
        }
    }
}
