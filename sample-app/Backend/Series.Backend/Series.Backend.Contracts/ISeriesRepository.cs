﻿using Series.Backend.Entities;
using System.Collections.Generic;


namespace Series.Backend.Contracts
{
    public interface ISeriesRepository
    {
        IEnumerable<TVSerie> GetSeries();
        TVSerie GetSerieById(int id);
        // TODO: Add insert serie
        // TODO: Add delete serie
        // TODO: Update serie
    }
}
