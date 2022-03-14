DROP PROCEDURE IF EXISTS uspGetWaypointsOnDate;

DELIMITER $

CREATE PROCEDURE uspGetWaypointsOnDate(
	selectedDate DATETIME,
	tripId INT
)

BEGIN
	SELECT waypointId, startDate, endDate, location, notes
	FROM waypoint
	WHERE waypoint.tripId = tripId AND (CONVERT(selectedDate, DATE) BETWEEN CONVERT(startDate, DATE) AND CONVERT(endDate, DATE));

END$

DELIMITER ;
