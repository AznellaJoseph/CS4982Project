DROP PROCEDURE IF EXISTS uspCreateWaypoint;

DELIMITER $

CREATE PROCEDURE uspCreateWaypoint(
    tripId INT,
    location VARCHAR(75),
    startTime DATETIME,
    endTime DATETIME
)
BEGIN
DECLARE numWaypointsInTrip INT;

SELECT COUNT(*) + 1
INTO numWaypointsInTrip
FROM waypoint
WHERE waypoint.tripId = tripId;

INSERT INTO waypoint (tripId, waypointNum, location, startTime, endTime)
VALUES (tripId, numWaypointsInTrip, location, startTime, endTime);

SELECT numWaypointsInTrip AS waypointNum;

END$
