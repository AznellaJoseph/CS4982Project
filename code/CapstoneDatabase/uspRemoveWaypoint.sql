DROP PROCEDURE IF EXISTS uspRemoveWaypoint;

DELIMITER $

CREATE PROCEDURE uspRemoveWaypoint(
	id INT
)
BEGIN
	DELETE FROM waypoint WHERE waypointId = id;

END$

DELIMITER ;
