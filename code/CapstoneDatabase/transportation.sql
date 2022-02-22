DROP TABLE IF EXISTS transportation;

CREATE TABLE transportation 
(
	transportationId INT UNSIGNED NOT NULL AUTO_INCREMENT,
	tripId INT UNSIGNED NOT NULL,
	method VARCHAR(50) NOT NULL,
	startTime DATETIME NOT NULL,
	endTime DATETIME NOT NULL,
	PRIMARY KEY (transportationId),
	CONSTRAINT fk_transportation_tripId
	FOREIGN KEY (tripId)
	REFERENCES trip(tripId)
	ON UPDATE CASCADE
	ON DELETE CASCADE
);
