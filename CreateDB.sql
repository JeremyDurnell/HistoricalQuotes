CREATE DATABASE HistoricalQuotes
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
GO

CREATE TABLE SymbolStats2011
(
	Symbol NVARCHAR(50) PRIMARY KEY,
	SharpeRatio FLOAT
)
GO

CREATE PROCEDURE [dbo].[CalculateSharpe]
(
	@Symbol VARCHAR(50),
	@StartDate DATETIME = '2011-01-03'
)
AS
BEGIN

--DECLARE @Symbol VARCHAR(50) 
--SET @Symbol = 'GLNG'

--DECLARE @StartDate DATETIME 
--SET @StartDate = '2011-01-03'

DECLARE @StartCumulativeReturn FLOAT
DECLARE @DailyReturn FLOAT

DECLARE @CumulativeReturn FLOAT


SELECT			@StartCumulativeReturn = AdjClose 
FROM			Quotes 
WHERE			Symbol = @Symbol 
AND				Timestamp = @StartDate

CREATE TABLE #TT
(
	[Timestamp] DATETIME PRIMARY KEY,
	AdjClose SMALLMONEY,
	CumulativeReturn FLOAT,
	DailyReturn FLOAT
)

INSERT INTO		#TT
SELECT			Timestamp,AdjClose,0,0
FROM			Quotes 
WHERE			Symbol = @Symbol

-- calculate cumulative return 
UPDATE #TT SET @CumulativeReturn = (AdjClose / @StartCumulativeReturn), CumulativeReturn = @CumulativeReturn

SET @CumulativeReturn = 1
-- calculate daily return
UPDATE #TT SET  @DailyReturn = (CumulativeReturn/@CumulativeReturn-1), @CumulativeReturn=CumulativeReturn, DailyReturn = @DailyReturn

-- calculate Sharpe Ratio for @Symbol
INSERT INTO SymbolStats2011
SELECT @Symbol AS Symbol, (SQRT (COUNT(*)) * AVG(DailyReturn) / STDEV(DailyReturn)) AS SharpeRatio FROM #TT

DROP TABLE #TT

END