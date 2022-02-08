DROP PROCEDURE IF EXISTS uspGetTripsForUser;

DELIMITER $

CREATE PROCEDURE uspGetTripsForUser(userId int)
BEGIN
    SELECT id, userId, name, startDate, endDate
    FROM trip
    WHERE trip.userId = userId;
END$