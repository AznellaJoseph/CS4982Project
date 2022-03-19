DROP PROCEDURE IF EXISTS uspGetTransportationOnDate;

DELIMITER $

CREATE PROCEDURE uspGetTransportationOnDate(
	selectedDate DATETIME,
	tripId INT
)

BEGIN
	SELECT transportationId, startDate, endDate, method, notes
	FROM transportation
	WHERE transportation.tripId = tripId AND (CONVERT(selectedDate, DATE) BETWEEN CONVERT(startDate, DATE) AND CONVERT(endDate, DATE));

END$

DELIMITER ;
