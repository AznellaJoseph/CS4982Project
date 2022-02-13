DROP PROCEDURE IF EXISTS uspGetWaypointsOnDate;

DELIMITER $

CREATE PROCEDURE uspGetWaypointsOnDate(
	selectedDate DATE,
	tripId int
)

BEGIN
	SELECT waypointNum, startTime, endTime, location, waypoint.notes
	FROM waypoint, trip
	WHERE waypoint.tripId = trip.id AND (CONVERT(startTime, DATE) = selectedDate OR CONVERT(endTime, DATE) = selectedDate);

END$
