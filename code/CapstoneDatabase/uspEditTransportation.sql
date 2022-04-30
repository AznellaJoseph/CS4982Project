DROP PROCEDURE IF EXISTS uspEditTransportation;

DELIMITER $

CREATE PROCEDURE uspEditTransportation(
	transportationId INT,
	method VARCHAR(75),
	notes VARCHAR(500),
	startDate DATETIME,
	endDate DATETIME
)

BEGIN
	UPDATE transportation
	SET transportation.method = method, transportation.startDate = startDate, transportation.endDate = endDate, transportation.notes = notes
	WHERE transportation.transportationId = transportationId;
END$

DELIMITER ;
