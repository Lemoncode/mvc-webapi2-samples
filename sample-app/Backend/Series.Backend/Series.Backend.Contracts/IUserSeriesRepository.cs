using Series.Backend.Entities;
using System.Collections.Generic;

namespace Series.Backend.Contracts
{
    public interface IUserSeriesRepository
    {
        IEnumerable<TVSerie> GetUserSeries(int userId);
        void AddUserSerie(int userId, int serieId);
    }
}
