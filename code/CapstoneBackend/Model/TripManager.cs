using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.Model
{
    public class TripManager
    {
        public Response<IList<Trip>> GetTripsByUser(int userId)
        {
            var dal = new TripDal();
            try
            {
                var trips = dal.GetTripsByUserId(userId);
                return new Response<IList<Trip>>
                {
                    Data = trips
                };
            }
            catch (MySqlException e)
            {
                return new Response<IList<Trip>>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
        }

        public Response<int> CreateTrip(int userId, string name, DateTime startDate, DateTime endDate)
        {
            if (startDate.CompareTo(endDate) > 0)
            {
                return new Response<int>
                {
                    StatusCode = 400,
                    ErrorMessage = "Start date of a trip cannot be after the end date."
                };
            }

            var dal = new TripDal();
            try
            {
                var tripId = dal.CreateTrip(userId, name, startDate, endDate);
                if (tripId is null)
                {
                    return new Response<int>
                    {
                        StatusCode = 500,
                        ErrorMessage = "Server failed to create trip."
                    };
                }
                return new Response<int>
                {
                    Data = (int)tripId
                };
            }
            catch (MySqlException e)
            {
                return new Response<int>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}