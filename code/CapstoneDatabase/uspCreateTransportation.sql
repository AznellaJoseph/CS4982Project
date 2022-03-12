DROP PROCEDURE IF EXISTS uspCreateTransportation;

DELIMITER $

CREATE PROCEDURE uspCreateTransportation(
	tripId int,
	method VARCHAR(50),
	startDate DATETIME,
	endDate DATETIME,
	notes VARCHAR(500)
)

BEGIN
	INSERT INTO transportation(tripId, method, startDate, endDate, notes)
	VALUES (tripId, method, startDate, endDate, notes);

	SELECT LAST_INSERT_ID();

END$

DELIMITER ;
