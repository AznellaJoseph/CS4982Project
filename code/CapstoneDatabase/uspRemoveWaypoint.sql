DROP PROCEDURE IF EXISTS uspRemoveWaypoint;

DELIMITER $

CREATE PROCEDURE uspRemoveWaypoint(
	waypointId INT
)
BEGIN
	DELETE FROM waypoint WHERE waypointId = waypointId;

END$

DELIMITER ;
