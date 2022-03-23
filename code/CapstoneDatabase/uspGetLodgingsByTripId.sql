DROP PROCEDURE IF EXISTS uspGetLodgingByTripId;

DELIMITER $

CREATE PROCEDURE uspGetLodgingByTripId(tripId INT)

BEGIN
	SELECT lodgingId, tripId, location, startDate, endDate, notes
	FROM lodging
	WHERE lodging.tripId = tripId;

END$

DELIMITER ;
