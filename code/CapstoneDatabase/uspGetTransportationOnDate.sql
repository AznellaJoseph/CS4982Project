DROP PROCEDURE IF EXISTS uspGetTransportationOnDate;

DELIMITER $

CREATE PROCEDURE uspGetTransportationOnDate(
	selectedDate DATETIME,
	tripId INT
)

BEGIN
	SELECT transportationId, transportation.startTime, transportation.endTime, method
	FROM transportation, trip
	WHERE transportation.tripId = trip.tripId AND (CONVERT(transportation.startTime, DATE) = CONVERT(selectedDate, DATE) OR CONVERT(transportation.endTime, DATE) = CONVERT(selectedDate, DATE));

END$

DELIMITER ;
