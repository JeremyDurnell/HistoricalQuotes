namespace HistoricalQuotes.Common.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using Entities;

    public class SqlRepository : IRepository
    {
        private readonly string _connectionString;

        public SqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region IRepository Members

        public void AddMany<T>(IEnumerable<T> entities) where T : Entity
        {
            DataTable dt = BuildDataTable(entities);

            using (var sbc = new SqlBulkCopy(_connectionString))
            {
                sbc.DestinationTableName = Pluaralize<T>();
                sbc.BatchSize = dt.Rows.Count;
                dt.Columns.Cast<DataColumn>().ToList().ForEach(
                    column => sbc.ColumnMappings.Add(column.ColumnName, column.ColumnName));
                sbc.WriteToServer(dt);
            }
        }

        public IEnumerable<T> Set<T>(Query query) where T : Entity, new()
        {
            throw new NotImplementedException();
        }

        #endregion

        private static string Pluaralize<T>() where T : Entity
        {
            return typeof (T).Name + "s";
        }

        /// <summary>
        /// This method assumes that all fields on entity exist in DB table, 1 entity per table,
        /// and that column names in DB match field names.  Obviously 3 very bad and very hasty
        /// assumptions (it can all be refactored later.)
        /// </summary>
        /// <returns></returns>
        /// <remarks>refactored to use http://highwayframework.github.com/Highway.Data/index.html, that is</remarks>
        private DataTable BuildDataTable<T>(IEnumerable<T> entities)
        {
            var dt = new DataTable();
            PropertyInfo[] propertyInfos = typeof (T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (string column in propertyInfos.Select(x => x.Name))
            {
                dt.Columns.Add(column);
            }

            foreach (T entity in entities)
            {
                DataRow dr = dt.NewRow();

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    dr[propertyInfo.Name] = propertyInfo.GetValue(entity, null);
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}