DROP PROCEDURE IF EXISTS uspAddWaypoint;

DELIMITER $

CREATE PROCEDURE uspAddWaypoint(
    IN tripId int,
    IN location VARCHAR(75),
    In startTime DATETIME,
    In endTime DATETIME,
    OUT waypointNum int
)
BEGIN

INSERT INTO waypoint (tripId, location, startTime, endTime)
VALUES (tripId, location, startTime, endTime);

SELECT MAX(waypointNum)
INTO waypointNum
FROM waypoint
WHERE waypoint.tripId = tripId
LIMIT 1;

END$