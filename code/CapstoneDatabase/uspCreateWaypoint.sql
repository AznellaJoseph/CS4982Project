DROP PROCEDURE IF EXISTS uspCreateWaypoint;

DELIMITER $

CREATE PROCEDURE uspCreateWaypoint(
    tripId INT,
    location VARCHAR(75),
    startDate DATETIME,
    endDate DATETIME
)
BEGIN

INSERT INTO waypoint (tripId, location, startDate, endDate)
VALUES (tripId, location, startDate, endDate);

SELECT LAST_INSERT_ID();

END$
