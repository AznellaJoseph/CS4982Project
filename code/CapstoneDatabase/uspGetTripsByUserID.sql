DROP PROCEDURE IF EXISTS uspGetTripsByUserID;

DELIMITER $

CREATE PROCEDURE uspGetTripsByUserID(userId int)
BEGIN
    SELECT id, name, startDate, endDate
    FROM trip
    WHERE trip.userId = userId;
END$