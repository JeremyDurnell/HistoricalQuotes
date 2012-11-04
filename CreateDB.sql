﻿CREATE DATABASE HistoricalQuotes
GO

USE HistoricalQuotes
GO

CREATE TABLE [Quotes]
(
	[Id] int PRIMARY KEY IDENTITY,
	[Symbol] NVARCHAR(50),
	[Timestamp] DATETIME NOT NULL,
	[Open] SMALLMONEY NOT NULL,
	[High] SMALLMONEY NOT NULL,
	[Low] SMALLMONEY NOT NULL,
	[AdjClose] SMALLMONEY NOT NULL,
	[Volume] INT NOT NULL
)