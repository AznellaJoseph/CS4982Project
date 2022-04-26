DROP PROCEDURE IF EXISTS uspEditLodging;

DELIMITER $

CREATE PROCEDURE uspEditLodging(
	lodgingId INT,
	location VARCHAR(75),
	notes VARCHAR(500),
	startDate DATETIME,
	endDate DATETIME
)

BEGIN
	UPDATE lodging
	SET lodging.location = location, lodging.startDate = startDate, lodging.endDate = endDate, lodging.notes = notes
	WHERE lodging.lodgingId = lodgingId;
END$

DELIMITER ;
