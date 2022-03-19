DROP PROCEDURE IF EXISTS uspRemoveLodging;

DELIMITER $

CREATE PROCEDURE uspRemoveLodging(
	lodgingId INT
)
BEGIN
	DELETE FROM lodging WHERE lodgingId = lodgingId;
END$

DELIMITER ;
