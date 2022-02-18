DROP PROCEDURE IF EXISTS uspGetTripsByTripId;

DELIMITER $

CREATE PROCEDURE uspGetTripsByTripId(tripId int)
BEGIN
    SELECT tripId, name, notes, startDate, endDate, userId
    FROM trip
    WHERE trip.tripId = tripId;
END$

DELIMITER ;
