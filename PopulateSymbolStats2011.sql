DECLARE symbolCursor CURSOR FOR 
SELECT DISTINCT Symbol FROM Quotes

OPEN symbolCursor

DECLARE @Symbol NVARCHAR(50)

FETCH NEXT FROM symbolCursor
INTO @Symbol
EXEC CalculateSharpe @Symbol

WHILE @@FETCH_STATUS = 0
BEGIN

	FETCH NEXT FROM symbolCursor
	INTO @Symbol
	EXEC CalculateSharpe @Symbol
	
END

CLOSE symbolCursor
DEALLOCATE symbolCursor

-- delete SharpeRatio where we don't have complete data (237 symbols)

WITH WTF AS 
(
	SELECT S.Symbol, COUNT(*) C FROM Quotes S 
	GROUP BY S.Symbol
)
DELETE FROM SymbolStats2011 WHERE Symbol IN 
(SELECT Symbol FROM WTF WHERE C <> 252)