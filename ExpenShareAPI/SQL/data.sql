USE [master]
GO
IF DB_ID('ExpenShare') IS NULL
CREATE DATABASE [ExpenShare]
GO
USE [ExpenShare]
GO

DROP TABLE IF EXISTS [user]
DROP TABLE IF EXISTS expense
DROP TABLE IF EXISTS event

CREATE TABLE [event] (
  [id] integer PRIMARY KEY IDENTITY,
  [name] varchar(255),
  [date] datetime,
  [comment] varchar(255)
)
GO

CREATE TABLE [user] (
  [id] integer PRIMARY KEY IDENTITY,
  [name] varchar(255),
  [email] varchar(255),
  [eventId] integer
)
GO

CREATE TABLE [expense] (
  [id] integer PRIMARY KEY IDENTITY,
  [name] varchar(255),
  [amount] decimal(10,2),
  [comment] varchar(255),
  [eventId] integer
)
GO

ALTER TABLE [expense] ADD FOREIGN KEY ([eventId]) REFERENCES [event] ([id])
GO

ALTER TABLE [user] ADD FOREIGN KEY ([eventId]) REFERENCES [event] ([id])
GO

INSERT INTO
event(name, date, comment)
VALUES
('Trip to Las Vegas', '2023-10-31', 'Girls'' Trip'),
('Breeden''s Orchard', '2023-10-15', '')

INSERT INTO
[user](name, email, eventId)
VALUES
('Cristi Neames', 'cris@email.com', 1),
('Deanna Hollifield', 'deeee@test.com', 1),
('Cristi Neames', 'cris@email.com', 2),
('Cliff Neames', 'clifford@mail.com', 2)

INSERT INTO
expense(name, amount, comment, eventId)
VALUES
('Hotel', 956.00, '', 1),
('Apple Cider', 12.00, '', 2),
('Flight', 888.50, 'Southwest', 1)


