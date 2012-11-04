namespace HistoricalQuotes.Common.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Entities;

    public class CsvRepository : IRepository
    {
        private readonly DirectoryInfo _dataDir;

        public CsvRepository(DirectoryInfo dataDir)
        {
            _dataDir = dataDir;

            if (!_dataDir.Exists)
            {
                throw new ArgumentException("directory does not exist");
            }
        }

        #region IRepository Members

        public void AddMany<T>(IEnumerable<T> entities) where T : Entity
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Set<T>(Query query) where T : Entity, new()
        {
            string filePath = Path.Combine(_dataDir.FullName, query.Symbol) + ".csv";

            if (!File.Exists(filePath))
            {
                return new T[0];
            }

            string text;

            using (var reader = new StreamReader(filePath))
            {
                text = reader.ReadToEnd();
            }

            string[] lines = text.Split(new[] {"\r\n"}, StringSplitOptions.None);

            var entities = new List<T>();
            string[] fields;
            Entity entity;

            foreach (string line in lines)
            {
                if (line.Length == 0)
                {
                    continue;
                }

                fields = line.Split(',');
                entity = new T();
                entity.OrdinalPopulate(fields);
                entities.Add((T) entity);
            }

            return entities;
        }

        #endregion
    }
}