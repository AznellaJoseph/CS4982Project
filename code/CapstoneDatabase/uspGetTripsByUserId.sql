DROP PROCEDURE IF EXISTS uspGetTripsByUserID;

DELIMITER $

CREATE PROCEDURE uspGetTripsByUserID(userId int)
BEGIN
    SELECT tripId, name, notes, startDate, endDate
    FROM trip
    WHERE trip.userId = userId;
END$

DELIMITER ;
