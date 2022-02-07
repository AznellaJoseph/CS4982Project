DROP PROCEDURE IF EXISTS uspCreateWaypoint;

DELIMITER $

CREATE PROCEDURE uspAddWaypoint(
    IN tripId int,
    IN location VARCHAR(75),
    In startTime DATETIME,
    In endTime DATETIME,
)
BEGIN

DECLARE @numWaypointsInTrip int;

SET @numWaypointsInTrip = (
    SELECT COUNT(*)
    FROM waypoint
    WHERE waypoint.tripId = tripId
) + 1;

INSERT INTO waypoint (tripId, waypointNum, location, startTime, endTime)
VALUES (tripId, @numWaypointsInTrip, location, startTime, endTime);

SELECT @numWaypointsInTrip;

END$
