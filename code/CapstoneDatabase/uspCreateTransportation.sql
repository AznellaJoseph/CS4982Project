DROP PROCEDURE IF EXISTS uspCreateTransportation;

DELIMITER $

CREATE PROCEDURE uspCreateTransportation(
	tripId int,
	method VARCHAR(50),
	startDate DATETIME,
	endDate DATETIME
)

BEGIN
	INSERT INTO transportation(tripId, method, startDate, endDate)
	VALUES (tripId, method, startDate, endDate);

	SELECT LAST_INSERT_ID();

END$

DELIMITER ;
