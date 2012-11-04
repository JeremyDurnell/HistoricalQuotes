namespace HistoricalQuotes.Common.Entities
{
    using System;

    public class Quote : Entity
    {
        public string Symbol { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal AdjClose { get; set; }
        public int Volume { get; set; }

        public override void OrdinalPopulate(string[] objs)
        {
            if (objs.Length >= 7)
            {
                Symbol = objs[0];
                Timestamp = DateTime.ParseExact(objs[1], "yyyyMMdd", null);
                Open = Decimal.Parse(objs[2]);
                High = Decimal.Parse(objs[3]);
                Low = Decimal.Parse(objs[4]);
                AdjClose = Decimal.Parse(objs[5]);
                Volume = int.Parse(objs[6]);
            }
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Symbol: {0}, Timestamp: {1}, Open: {2}, High: {3}, Low: {4}, Volume: {5}, AdjClose: {6}", Symbol,
                    Timestamp, Open, High, Low, Volume, AdjClose);
        }
    }
}