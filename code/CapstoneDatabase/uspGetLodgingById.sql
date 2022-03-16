DROP PROCEDURE IF EXISTS uspGetLodgingById;

DELIMITER $

CREATE PROCEDURE uspGetLodgingById(lodgingId INT)

BEGIN
	SELECT lodgingId, tripId, location, startDate, endDate, notes
	FROM lodging
	WHERE lodging.lodgingId = lodgingId;

END$

DELIMITER ;
