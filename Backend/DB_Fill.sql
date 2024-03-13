--ADD Test Data

INSERT INTO "Genres"
VALUES(1, 'Pop'),
      (2, 'Rock'),
      (3, 'Dance'),
      (4, 'Latin')
;

INSERT INTO "Tracks"
VALUES(1, 'Sunflower', 1),
      (2, 'Spoil My Night (feat. Swae Lee)',1),
      (3, 'Stay',1),
      (4, 'Circles',1),
      (5, 'In the End',2),
      (6, 'Numb',2),
      (7, 'CASTLE OF GLASS',2),
      (8, 'Believer',1),
      (9, 'Continual (feat. Cory Henry)',1),
      (10, 'Thunder',1)
;

INSERT INTO "Artists"
VALUES(1, 'Post Malone'),
      (2, 'Swae Lee'),
      (3, 'Linkin Park'),
      (4, 'Imagine Dragons'),   
      (5, 'Cory Henry')   
;

INSERT INTO "TrackArtists"
VALUES(1, 1),
      (2, 1),
      (2, 2),
      (3, 1),
      (4, 1),

      (5, 3),
      (6, 3),
      (7, 3),

      (8, 4),
      (9, 4),
      (9, 5),
      (10, 4)
;
