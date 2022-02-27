DROP PROCEDURE IF EXISTS uspTransportationTrip;

DELIMITER $

CREATE PROCEDURE uspRemoveTransportation(
	trasportationId int,
)

BEGIN
	DELETE FROM transportation WHERE trasportationId = trasportationId;
END$

DELIMITER ;


