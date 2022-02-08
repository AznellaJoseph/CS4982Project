using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.Model
{
    public class TripManager
    {
        
        private readonly TripDal _dal;

        public TripManager() : this(new TripDal())
        {
        }

        public TripManager(TripDal dal)
        {
            _dal = dal;
        }
        
        public Response<IList<Trip>> GetTripsByUser(int userId)
        {
            try
            {
                var trips = this._dal.GetTripsByUserId(userId);
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
            
            try
            {
                var tripId = this._dal.CreateTrip(userId, name, startDate, endDate);
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