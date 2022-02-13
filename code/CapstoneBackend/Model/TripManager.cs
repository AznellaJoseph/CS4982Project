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
        
        public virtual Response<IList<Trip>> GetTripsByUser(int userId)
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
            catch (Exception e)
            {
                return new Response<IList<Trip>>
                {
                    StatusCode = 500,
                    ErrorMessage = e.Message
                };
            }
        }

        public virtual Response<int> CreateTrip(int userId, string name, string? notes, DateTime startDate, DateTime endDate)
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
                var tripId = this._dal.CreateTrip(userId, name, notes, startDate, endDate);
                return new Response<int>
                {
                    Data = tripId
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
            catch (Exception e)
            {
                return new Response<int>
                {
                    StatusCode = 500,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}