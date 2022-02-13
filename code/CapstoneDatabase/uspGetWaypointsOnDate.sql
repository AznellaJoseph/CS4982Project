DROP PROCEDURE IF EXISTS uspGetWaypointsOnDate;

DELIMITER $

CREATE PROCEDURE uspGetWaypointsOnDate(
	selectedDate DATE,
	tripId int
)

BEGIN
	SELECT waypointId, startDate, endDate, location, waypoint.notes
	FROM waypoint, trip
	WHERE waypoint.tripId = trip.tripId AND (CONVERT(startDate, DATE) = selectedDate OR CONVERT(endDate, DATE) = selectedDate);

END$
