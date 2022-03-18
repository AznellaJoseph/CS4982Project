DROP PROCEDURE IF EXISTS uspGetWaypointsByTripId;

DELIMITER $

CREATE PROCEDURE uspGetWaypointsByTripId(
	tripId INT
)
BEGIN
        SELECT waypointId, location, startDate, endDate, notes
        FROM waypoint
        WHERE waypoint.tripId = tripId;
END$

DELIMITER ;
