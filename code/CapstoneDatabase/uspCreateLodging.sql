DROP PROCEDURE IF EXISTS uspCreateLodging;

DELIMITER $

CREATE PROCEDURE uspCreateLodging(
	tripId INT,
	location VARCHAR(75),
	notes VARCHAR(500),
	startDate DATETIME,
	endDate DATETIME
)
BEGIN
	INSERT INTO lodging (tripId, location, startDate, endDate, notes)
	VALUES (tripId, location, startDate, endDate, notes);

SELECT LAST_INSERT_ID();

END$

DELIMITER ;
