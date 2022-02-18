DROP PROCEDURE IF EXISTS uspCreateWaypoint;

DELIMITER $

CREATE PROCEDURE uspCreateWaypoint(
    tripId INT,
    location VARCHAR(75),
    notes VARCHAR(500),
    startDate DATETIME,
    endDate DATETIME
)
BEGIN
INSERT INTO waypoint (tripId, location, startDate, endDate, notes)
VALUES (tripId, location, startDate, endDate, notes);

SELECT LAST_INSERT_ID();

END$

DELIMITER ;
