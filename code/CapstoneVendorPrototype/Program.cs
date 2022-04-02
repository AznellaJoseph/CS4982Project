using System;

namespace CapstoneVendorPrototype
{
    /// <summary>
    ///     A Prototype Program to test Vendor API
    /// </summary>
    public static class Program
    {
        /// <summary>
        ///     The main entry to test the prototype.
        /// </summary>
        /// <param name="args">the args for the execution</param>
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter a latitude:");
            var latitude = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter a longitude:");
            var longitude = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter a radius:");
            var radius = Convert.ToInt32(Console.ReadLine());
            PlaceClient client = new();
            var result = client.GetPointsOfInterest(latitude, longitude, radius);
            Console.WriteLine("Points of Interest:");
            foreach (var aPlace in result.Result.Results)
            {
                Console.WriteLine(aPlace.Name);
                Console.WriteLine(aPlace.Location.Address);
                Console.WriteLine(aPlace.Location.Region);
            }
        }
    }
}