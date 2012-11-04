This utility will connect to a SQL Server database and load a table named "Quotes" from external .csv files.

The .csv files must be in the following format: (I call it the Yahoo! Finance format)

Symbol, Date (YYYYMMdd,) Open, High, Low, AdjClose, Volume

The data directory and connection string are hard coded in Program.cs (update according to your environment.)

The CreateDB.sql script can be used to create the database.

The data files are included (~100 MB.)  If you don't want the data files, then check out the following revision:
7b40adec1cfd891c4cedd42d18da2ef268dd4750 
