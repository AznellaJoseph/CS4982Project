DROP PROCEDURE IF EXISTS uspGetTransportationById;

DELIMITER $

CREATE PROCEDURE uspGetTransportationById(
	transportationId INT
)

BEGIN
	SELECT transportationId, tripId, method, startDate, endDate, notes
	FROM transportation
	WHERE transportation.transportationId = transportationId;

END$

DELIMITER ;
