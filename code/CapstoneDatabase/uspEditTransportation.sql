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
	SET method = method, startDate = startDate, endDate = endDate, notes = notes
	WHERE transportationId = transportationId;
END$

DELIMITER ;
