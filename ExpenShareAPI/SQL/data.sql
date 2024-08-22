USE [master]
GO
IF DB_ID('ExpenShare') IS NULL
CREATE DATABASE [ExpenShare]
GO
USE [ExpenShare]
GO

DROP TABLE IF EXISTS userEvent
DROP TABLE IF EXISTS expense
DROP TABLE IF EXISTS event
DROP TABLE IF EXISTS tokens
DROP TABLE IF EXISTS [user]


CREATE TABLE [user] (
  [id] integer PRIMARY KEY IDENTITY(1,1),     
  [userName] VARCHAR(255) UNIQUE NOT NULL, 
  [passwordHash] VARCHAR(255) NOT NULL,   
  [email] VARCHAR(255) UNIQUE,            
  [fullName] VARCHAR(255),               
  [createdAt] DATETIME DEFAULT GETDATE(),  
  [ipdatedAt] DATETIME DEFAULT GETDATE(),  
  [lastLogin] DATETIME                     
)
GO

CREATE TABLE [tokens] (
    [tokenId] INT PRIMARY KEY IDENTITY(1,1),
    [refreshToken] VARCHAR(255),                  
    [expiresAt] DATETIME,                          
    [createdAt] DATETIME DEFAULT GETDATE(),        
    [revokedAt] DATETIME,
    [userId] INT,
    CONSTRAINT UC_RefreshToken UNIQUE (RefreshToken) 
)
GO

CREATE TABLE [event] (
  [id] integer PRIMARY KEY IDENTITY,
  [name] varchar(255),
  [date] datetime,
  [comment] varchar(255)
)
GO

CREATE TABLE [userEvent] (
  [id] integer PRIMARY KEY IDENTITY,
  [userId] integer,
  [eventId] integer
)

CREATE TABLE [expense] (
  [id] integer PRIMARY KEY IDENTITY,
  [name] varchar(255),
  [amount] decimal(10,2),
  [comment] varchar(255),
  [eventId] integer
)
GO

ALTER TABLE [userEvent] ADD FOREIGN KEY ([userId]) REFERENCES [user] ([id])

ALTER TABLE [userEvent] ADD FOREIGN KEY ([eventId]) REFERENCES [event] ([id])

ALTER TABLE [expense] ADD FOREIGN KEY ([eventId]) REFERENCES [event] ([id])

ALTER TABLE [tokens] ADD FOREIGN KEY ([userId]) REFERENCES [user] ([id])
GO

INSERT INTO
[user](userName, passwordHash, email, fullName, createdAt)
VALUES
('CrisN24', 'hashedpassword_1', 'cris@email.com', 'Cristi Neames', GETDATE()),
('DHolli66', 'hashedpassword_2', 'deeee@test.com', 'Deanna Hollifield', GETDATE()),
('cliffy2', 'hashedpassword_3', 'clifford@mail.com', 'Cliff Neames', GETDATE())

INSERT INTO
[tokens](refreshToken, expiresAt, createdAt, userId)
VALUES
('sampleHashedToken123', '2024-12-31', GETDATE(), 1),
('sampleHashedToken456', '2024-12-31', GETDATE(), 2),
('sampleHashedToken789', '2024-12-31', GETDATE(), 3)

INSERT INTO
[event](name, date, comment)
VALUES
('Trip to Las Vegas', '2023-10-31', 'Girls'' Trip'),
('Breeden''s Orchard', '2023-10-15', '')

INSERT INTO
expense(name, amount, comment, eventId)
VALUES
('Hotel', 956.00, '', 1),
('Apple Cider', 12.00, '', 2),
('Flight', 888.50, 'Southwest', 1)

INSERT INTO
userEvent(userId, eventId)
VALUES
(1,1),
(1,2),
(2,1),
(3,2)