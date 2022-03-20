DROP PROCEDURE IF EXISTS uspRemoveWaypoint;

DELIMITER $

CREATE PROCEDURE uspRemoveWaypoint(
	waypointId INT
)
BEGIN
	DELETE FROM waypoint WHERE waypoint.waypointId = waypointId;
END$

DELIMITER ;
