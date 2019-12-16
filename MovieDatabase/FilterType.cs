using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDatabase
{
    public enum FilterType
    {
        ftTitlesByName,
        ftTitlesByPerson,
        ftSerieForEpisode,
        ftEpisodesForSerie,
        ftPersonsByName,
        ftPersonsByTitle
    }
}
