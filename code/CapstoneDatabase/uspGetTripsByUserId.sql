DROP PROCEDURE IF EXISTS uspGetTripsByUserId;

DELIMITER $

CREATE PROCEDURE uspGetTripsByUserId(userId int)
BEGIN
    SELECT tripId, name, notes, startDate, endDate
    FROM trip
    WHERE trip.userId = userId;
END$

DELIMITER ;
