CREATE TABLE [Map] (
  [ID] INTEGER PRIMARY KEY, 
  [Caption] CHAR(64) NOT NULL, 
  [Picture] BINARY, 
  [Left] INTEGER NOT NULL, 
  [Top] INTEGER NOT NULL, 
  [Right] INTEGER NOT NULL, 
  [Bottom] INTEGER NOT NULL
);

CREATE TABLE [Layer] (
  [ID] INTEGER PRIMARY KEY,
  [MapID] INTEGER NOT NULL,
  [Level] INTEGER NOT NULL,
  [Name] CHAR(64) NOT NULL,
  [Picture] BINARY,
  FOREIGN KEY(MapID) REFERENCES Map(ID) ON DELETE CASCADE
);

CREATE TABLE Quest (
  [ID] INTEGER PRIMARY KEY, 
  [MapID] INTEGER NOT NULL,
  [Name] CHAR(64) NOT NULL,
  [Trader] INTEGER NOT NULL, 
  FOREIGN KEY(MapID) REFERENCES Map(ID) ON DELETE CASCADE
);

CREATE TABLE Marker (
  [ID] INTEGER PRIMARY KEY,
  [MapID] INTEGER NOT NULL,
  [QuestID] INTEGER NULL,
  [Caption] CHAR(64) NOT NULL,
  [Kind] INTEGER NOT NULL,
  [Left] INTEGER NOT NULL,
  [Top] INTEGER NOT NULL,
  FOREIGN KEY(MapID) REFERENCES Map(ID) ON DELETE CASCADE,
  FOREIGN KEY(QuestID) REFERENCES Quest(ID) ON DELETE CASCADE
);

CREATE TABLE [Resource] (
  [ID] INTEGER PRIMARY KEY, 
  [MarkerID] INTEGER NULL,
  [Kind] INTEGER NOT NULL,
  [Description] CHAR(512) NOT NULL, 
  [Picture] BINARY NULL,
  FOREIGN KEY(MarkerID) REFERENCES Marker(ID) ON DELETE CASCADE
);

CREATE TABLE [QuestItem] (
  [ID] INTEGER PRIMARY KEY,
  [ResourceID] INTEGER NOT NULL,
  [MarkerID] INTEGER NOT NULL,
  FOREIGN KEY(ResourceID) REFERENCES Resource(ID) ON DELETE CASCADE,
  FOREIGN KEY(MarkerID) REFERENCES Marker(ID) ON DELETE CASCADE
);

CREATE TABLE [Profile] (
  [ID] INTEGER PRIMARY KEY, 
  [Name] CHAR(64) NOT NULL
);

CREATE TABLE [QuestTracker] (
  [ID] INTEGER PRIMARY KEY, 
  [QuestID] INTEGER NULL, 
  [MarkerID] INTEGER NULL, 
  [Status] INTEGER NOT NULL,
  FOREIGN KEY(QuestID) REFERENCES Quest(ID) ON DELETE CASCADE,
  FOREIGN KEY(MarkerID) REFERENCES Marker(ID) ON DELETE CASCADE
);

CREATE TABLE [Options] (
  [ID] INTEGER PRIMARY KEY, 
  [ProfileID] INTEGER NULL, 
  [DataPath] CHAR(1024) NOT NULL, 
  [SreenshotPath] CHAR(1024) NOT NULL,
  [TrackLocation] INTEGER NULL
);