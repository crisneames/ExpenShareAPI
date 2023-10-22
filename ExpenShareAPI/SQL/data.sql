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
DROP TABLE IF EXISTS [user]

CREATE TABLE [user] (
  [id] integer PRIMARY KEY IDENTITY,
  [name] varchar(255),
  [email] varchar(255)
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
GO

INSERT INTO
[user](name, email)
VALUES
('Cristi Neames', 'cris@email.com'),
('Deanna Hollifield', 'deeee@test.com'),
('Cliff Neames', 'clifford@mail.com')

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