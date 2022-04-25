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
	SET location = location, startDate = startDate, endDate = endDate, notes = notes
	WHERE lodgingId = lodgingId;
END$

DELIMITER ;
