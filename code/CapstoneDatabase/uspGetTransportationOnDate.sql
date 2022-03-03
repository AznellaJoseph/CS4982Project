DROP PROCEDURE IF EXISTS uspGetTransportationOnDate;

DELIMITER $

CREATE PROCEDURE uspGetTransportationOnDate(
	selectedDate DATETIME,
	tripId INT
)

BEGIN
	SELECT transportationId, transportation.startDate, transportation.endDate, method
	FROM transportation, trip
	WHERE transportation.tripId = trip.tripId AND (CONVERT(selectedDate, DATE) BETWEEN CONVERT(transportation.startDate, DATE) AND CONVERT(transportation.endDate, DATE));

END$

DELIMITER ;
