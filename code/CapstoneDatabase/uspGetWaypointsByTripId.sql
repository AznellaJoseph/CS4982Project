DROP PROCEDURE IF EXISTS uspGetWaypointsByTripId;

DELIMITER $

CREATE PROCEDURE uspGetWaypointsByTripId(
	tripId INT UNSIGNED
)
BEGIN
        SELECT waypointNum, location, startTime, endTime
        FROM waypoint
        WHERE waypoint.tripId = tripId;
END$
