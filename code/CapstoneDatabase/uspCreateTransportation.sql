DROP PROCEDURE IF EXISTS uspCreateTransportation;

DELIMITER $

CREATE PROCEDURE uspCreateTransportation(
	tripId int,
	method VARCHAR(50),
	startTime DATETIME,
	endTime DATETIME
)

BEGIN
	INSERT INTO transportation(tripId, method, startTime, endTime)
	VALUES (tripId, method, startTime, endTime);

	SELECT LAST_INSERT_ID();

END$

DELIMITER ;
