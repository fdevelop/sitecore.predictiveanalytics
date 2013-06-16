namespace Sitecore.PredictivePersonalization.Data.DataAccess.SqlServer
{
  using Sitecore.Analytics.Data.DataAccess.DataAdapters;
  using Sitecore.Diagnostics;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class SqlTrailsBuilder : ITrailsBuilderService
  {
    private string[] cacheStateNames;
    
    public string[] GetStateNames()
    {
      if (this.cacheStateNames == null)
      {
        var data = DataAdapterManager.Sql.ReadMany<string>(
          "select distinct Url from Pages order by Url",
          reader => (string)reader.InnerReader[0], 
          new object[] { });

        this.cacheStateNames = data.ToArray();
      }

      return this.cacheStateNames;
    }

    public VisitTrail[] GetTrails()
    {
      var visits = DataAdapterManager.Sql.ReadMany<Guid>(
          "select VisitId from Visits",
          reader => (Guid)reader.InnerReader[0],
          new object[] { });

      var visitsCollection = visits.ToArray();

      VisitTrail[] trails = new VisitTrail[visitsCollection.Length];

      for (int i = 0; i < visitsCollection.Length; i++)
      {
        var currSiteVisit = visitsCollection[i];

        var pagesInVisit = DataAdapterManager.Sql.ReadMany(
          "select Url, DateTime from Pages where Pages.VisitId = @VisitId order by Pages.VisitPageIndex",
          reader => new { Url = (string)reader.InnerReader[0], DateTime = (DateTime)reader.InnerReader[1] },
          new object[] { "VisitId", currSiteVisit });

        var pagesInVisitLocal = pagesInVisit.ToArray();

        var visitIndexes = new List<long>();
        var pageDelays = new List<TimeSpan>();

        for (int j = 0; j < pagesInVisitLocal.Length; j++)
        {
          var page = pagesInVisitLocal[j];
          long idx = Array.BinarySearch<string>(this.GetStateNames(), page.Url, StringComparer.InvariantCultureIgnoreCase);
          if (idx < 0)
          {
            Log.Debug("Seems to be error, failed to locate state: " + page);
            throw new Exception("Binary search on mapping pages with indexes failed.");
          }

          visitIndexes.Add(idx);
          if (j > 0)
          {
            pageDelays.Add(page.DateTime - pagesInVisitLocal[j - 1].DateTime);
          }
        }

        trails[i] = new VisitTrail(visitIndexes.ToArray(), pageDelays.ToArray());
      }

      return trails;
    }

    public SqlTrailsBuilder()
    {
      this.GetStateNames();
      Log.Debug("SqlTrailsBuilder initialized.");
    }
  }
}
