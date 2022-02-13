DROP PROCEDURE IF EXISTS uspCreateTrip;

DELIMITER $

CREATE PROCEDURE uspCreateTrip(
	userId int,
	name VARCHAR(45),
	startDate DATE,
	endDate DATE,
	notes VARCHAR(500)
)

BEGIN
	INSERT INTO trip(userId, name, startDate, endDate, notes)
	VALUES (userId, name, startDate, endDate, notes);

	SELECT LAST_INSERT_ID();

END$
