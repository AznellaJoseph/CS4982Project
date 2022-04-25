DROP PROCEDURE IF EXISTS uspEditWaypoint;

DELIMITER $

CREATE PROCEDURE uspEditWaypoint(
	location VARCHAR(75),
	notes VARCHAR(500),
	startDate DATETIME,
	endDate DATETIME
)

BEGIN
	UPDATE waypoint
	SET location = location, startDate = startDate, endDate = endDate, notes = notes
	WHERE waypointId = waypointId;
END$

DELIMITER ;
