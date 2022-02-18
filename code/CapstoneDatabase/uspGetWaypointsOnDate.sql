DROP PROCEDURE IF EXISTS uspGetWaypointsOnDate;

DELIMITER $

CREATE PROCEDURE uspGetWaypointsOnDate(
	selectedDate DATETIME,
	tripId INT
)

BEGIN
	SELECT waypointId, waypoint.startDate, waypoint.endDate, location, waypoint.notes
	FROM waypoint, trip
	WHERE waypoint.tripId = trip.tripId AND (CONVERT(waypoint.startDate, DATE) = CONVERT(selectedDate, DATE) OR CONVERT(waypoint.endDate, DATE) = CONVERT(selectedDate, DATE));

END$

DELIMITER ;
