namespace HistoricalQuotes.Common.Data
{
    using System.Collections.Generic;
    using Entities;

    public interface IRepository
    {
        void AddMany<T>(IEnumerable<T> entities) where T : Entity;
        IEnumerable<T> Set<T>(Query query) where T : Entity, new();
    }
}