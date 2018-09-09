using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Contracts
{
    public interface ISeriesRepository : IDisposable
    {
        Task<Serie> GetSerieByIdAsync(int id);
        IEnumerable<Serie> GetSeries();
    }
}
