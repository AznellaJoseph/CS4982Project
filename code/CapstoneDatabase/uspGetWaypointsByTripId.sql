DROP PROCEDURE IF EXISTS uspGetWaypointsByTripId;

DELIMITER $

CREATE PROCEDURE uspGetWaypointsByTripId(
	tripId INT UNSIGNED
)
BEGIN
        SELECT waypointId, location, startTime, endTime
        FROM waypoint
        WHERE waypoint.tripId = tripId;
END$
