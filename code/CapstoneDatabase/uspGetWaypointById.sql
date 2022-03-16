DROP PROCEDURE IF EXISTS uspGetWaypointById;

DELIMITER $

CREATE PROCEDURE uspGetWaypointById(waypointId INT)

BEGIN
	SELECT waypointId, tripId, location, startDate, endDate, notes
	FROM waypoint
	WHERE waypoint.waypointId = waypointId;

END$

DELIMITER ;
