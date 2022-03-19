DROP PROCEDURE IF EXISTS uspGetLodgingsOnDate;

DELIMITER $

CREATE PROCEDURE uspGetLodgingsOnDate(
	tripId INT,
	selectedDate DATETIME
)

BEGIN
	SELECT lodgingId, tripId, location, startDate, endDate, notes
	FROM lodging
	WHERE lodging.tripId = tripId AND (CONVERT(selectedDate, DATE) BETWEEN CONVERT(startDate, DATE) AND CONVERT(endDate, DATE));

END$

DELIMITER ;
