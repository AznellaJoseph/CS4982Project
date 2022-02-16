DROP PROCEDURE IF EXISTS uspGetWaypointsOnDate;

DELIMITER $

CREATE PROCEDURE uspGetWaypointsOnDate(
	selectedDate DATE,
	tripId int
)

BEGIN
	SELECT waypointId, waypoint.startDate, waypoint.endDate, location
	FROM waypoint, trip
	WHERE waypoint.tripId = trip.tripId AND (CONVERT(waypoint.startDate, DATE) = selectedDate OR CONVERT(waypoint.endDate, DATE) = selectedDate);

END$

DELIMITER ;
