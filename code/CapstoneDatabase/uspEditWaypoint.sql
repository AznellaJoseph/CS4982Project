DROP PROCEDURE IF EXISTS uspEditWaypoint;

DELIMITER $

CREATE PROCEDURE uspEditWaypoint(
	waypointId INT,
	location VARCHAR(75),
	notes VARCHAR(500),
	startDate DATETIME,
	endDate DATETIME
)

BEGIN
	UPDATE waypoint
	SET waypoint.location = location, waypoint.startDate = startDate, waypoint.endDate = endDate, waypoint.notes = notes
	WHERE waypoint.waypointId = waypointId;
END$

DELIMITER ;
