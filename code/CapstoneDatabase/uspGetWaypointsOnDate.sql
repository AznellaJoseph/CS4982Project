DROP PROCEDURE IF EXISTS uspGetWaypointsOnDate;

DELIMITER $

CREATE PROCEDURE uspGetWaypointsOnDate(
	selectedDate DATETIME,
	tripId INT
)

BEGIN
	SELECT waypointId, waypoint.startDate, waypoint.endDate, location, waypoint.notes
	FROM waypoint, trip
	WHERE waypoint.tripId = trip.tripId AND (CONVERT(selectedDate, DATE) BETWEEN CONVERT(waypoint.startDate, DATE) AND CONVERT(waypoint.endDate, DATE));

END$

DELIMITER ;
