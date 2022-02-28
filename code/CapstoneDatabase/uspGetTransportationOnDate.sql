DROP PROCEDURE IF EXISTS uspGetTransportationOnDate;

DELIMITER $

CREATE PROCEDURE uspGetTransportationOnDate(
	selectedDate DATETIME,
	tripId INT
)

BEGIN
	SELECT transportationId, transportation.startDate, transportation.endDate, method
	FROM transportation, trip
	WHERE transportation.tripId = trip.tripId AND (CONVERT(transportation.startDate, DATE) = CONVERT(selectedDate, DATE) OR CONVERT(transportation.endDate, DATE) = CONVERT(selectedDate, DATE));

END$

DELIMITER ;
