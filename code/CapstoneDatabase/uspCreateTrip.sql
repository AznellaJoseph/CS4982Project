DROP PROCEDURE IF EXISTS uspCreateTrip;

DELIMITER $

CREATE PROCEDURE uspCreateTrip(
	userId int,
	name VARCHAR(45),
	startDate DATE,
	endDate DATE
)

BEGIN
	INSERT INTO trip(userId, name, startDate, endDate)
	VALUES (userId, name, startDate, endDate);

	SELECT LAST_INSERT_ID();

END$
