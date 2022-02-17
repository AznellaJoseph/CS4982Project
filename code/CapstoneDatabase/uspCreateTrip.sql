DROP PROCEDURE IF EXISTS uspCreateTrip;

DELIMITER $

CREATE PROCEDURE uspCreateTrip(
	userId int,
	name VARCHAR(45),
	notes VARCHAR(500),
	startDate DATE,
	endDate DATE
)

BEGIN
	INSERT INTO trip(userId, name, notes, startDate, endDate)
	VALUES (userId, name, notes, startDate, endDate);

	SELECT LAST_INSERT_ID();

END$

DELIMITER ;
