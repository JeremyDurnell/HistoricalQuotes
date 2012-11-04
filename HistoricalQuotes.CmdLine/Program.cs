using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HistoricalQuotes.Common.Data;
using HistoricalQuotes.Common.Entities;

namespace HistoricalQuotes.CmdLine
{
    public class Program
    {
        private static void Main()
        {
            var dataDir = new DirectoryInfo(@"C:\historicaldata");
            var csvRepo = new CsvRepository(dataDir);
            var sqlRepo = new SqlRepository(@"Data Source=.;Initial Catalog=HistoricalQuotes;Integrated Security=True");
            string processedFilesDir = Path.Combine(@"C:\historicaldata", "processed");

            if (!Directory.Exists(processedFilesDir))
            {
                Directory.CreateDirectory(processedFilesDir);
            }

            FileInfo[] files = dataDir.GetFiles();

            foreach (FileInfo file in files)
            {
                string symbol = Path.GetFileNameWithoutExtension(file.Name);
                Console.Write(symbol);

                IEnumerable<Quote> quotes = csvRepo.Set<Quote>(new Query {Symbol = symbol});
                var fromDate = new DateTime(2011, 1, 1);
                var toDate = new DateTime(2011, 12, 31);

                // we're only interested in 2011 data
                sqlRepo.AddMany(quotes.Where(q => q.Timestamp >= fromDate && q.Timestamp <= toDate));

                string processedFileName = Path.Combine(processedFilesDir, file.Name);

                if (File.Exists(processedFileName))
                {
                    Console.Write("...Error (symbol already processed)");
                    Console.WriteLine();
                    continue;
                }

                if (file.Exists)
                {
                    file.MoveTo(processedFileName);
                }

                Console.Write("...Done");
                Console.WriteLine();
            }
        }
    }
}