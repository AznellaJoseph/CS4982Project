DROP TABLE IF EXISTS lodging;

CREATE TABLE lodging
(
    lodgingId INT UNSIGNED NOT NULL AUTO_INCREMENT,
    tripId INT UNSIGNED NOT NULL, 
    location VARCHAR(75) NOT NULL,
    startDate DATETIME NOT NULL,
    endDate DATETIME,
    notes VARCHAR(500),
    PRIMARY KEY (lodgingId),
    CONSTRAINT fk_lodging_tripId
        FOREIGN KEY (tripId)
            REFERENCES trip(tripId)
            ON UPDATE CASCADE
            ON DELETE CASCADE
);
