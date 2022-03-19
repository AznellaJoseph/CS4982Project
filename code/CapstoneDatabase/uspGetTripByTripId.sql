DROP PROCEDURE IF EXISTS uspGetTripByTripId;

DELIMITER $

CREATE PROCEDURE uspGetTripByTripId(tripId int)
BEGIN
    SELECT tripId, name, notes, startDate, endDate, userId
    FROM trip
    WHERE trip.tripId = tripId;
END$

DELIMITER ;
