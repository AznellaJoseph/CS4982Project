DROP TABLE IF EXISTS transportation;

CREATE TABLE transportation (
	transportationId INT UNSIGNED NOT NULL AUTO_INCREMENT,
	startTime DATETIME NOT NULL,
	endTime DATETIME NOT NULL,
	PRIMARY KEY (transportationId)
);
