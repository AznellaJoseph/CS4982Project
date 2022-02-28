DROP PROCEDURE IF EXISTS uspRemoveTransportation;

DELIMITER $

CREATE PROCEDURE uspRemoveTransportation(
	transportationId INT
)
BEGIN
	DELETE FROM transportation WHERE transportationId = transportationId;
END$

DELIMITER ;
