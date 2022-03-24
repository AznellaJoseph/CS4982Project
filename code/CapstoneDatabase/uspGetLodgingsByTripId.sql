DROP PROCEDURE IF EXISTS uspGetLodgingsByTripId;

DELIMITER $

CREATE PROCEDURE uspGetLodgingsByTripId(tripId INT)

BEGIN
	SELECT lodgingId, tripId, location, startDate, endDate, notes
	FROM lodging
	WHERE lodging.tripId = tripId;

END$

DELIMITER ;
