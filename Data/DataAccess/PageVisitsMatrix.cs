namespace Sitecore.PredictivePersonalization.Data.DataAccess
{
  using System.IO;
  using System;
  using Sitecore.Diagnostics;

  public class PageVisitsMatrix
  {
    private double[,] mx;
    private string[] stateNames;

    private double[,] MakeMatrix(VisitTrail[] trails)
    {
      Assert.IsNotNull(this.stateNames, "States must be filled");

      double[,] data = new double[trails.Length, this.stateNames.Length];
      for (int i = 0; i < trails.Length; i++)
      {
        for (int j = 0; j < trails[i].PageCount; j++)
        {
          data.SetValue(1, i, trails[i][j]);
        }
      }

      return data;
    }

    public PageVisitsMatrix(ITrailsBuilderService service)
    {
      Log.Debug("PageVisits matrix building started.");
      this.stateNames = service.GetStateNames();
      this.mx = this.MakeMatrix(service.GetTrails());
      Log.Debug("PageVisits matrix building ready.");
    }

    public string[] StateNames
    {
      get
      {
        return this.stateNames;
      }
    }

    public double[,] Matrix
    {
      get
      {
        return this.mx;
      }
    }

    public DataMatrix ToDataMatrix()
    {
      Log.Debug("CLustering: converting PageVisitsMatrix to DataMatrix start");
      double[][] tmp = new double[this.mx.GetLength(0)][];
      for (int i = 0; i < this.mx.GetLength(0); i++)
      {
        tmp[i] = new double[this.mx.GetLength(1)];
        for (int j = 0; j < this.mx.GetLength(1); j++)
        {
          tmp[i][j] = this.mx[i, j];
        }
      }

      Log.Debug("CLustering: converting PageVisitsMatrix to DataMatrix end!");
      return new DataMatrix(tmp);
    }

    public void SaveToFile(string path)
    {
      using (StreamWriter sw = File.CreateText(path))
      {
        for (int i = 0; i < this.stateNames.Length; i++)
        {
          sw.Write(this.stateNames[i]);

          if (i != this.stateNames.Length - 1)
          {
            sw.Write(" ");
          }
        }
        sw.WriteLine();

        for (int i = 0; i < this.mx.GetLength(0); i++)
        {
          for (int j = 0; j < this.mx.GetLength(1); j++)
          {
            sw.Write(this.mx[i, j].ToString("F1"));

            if (j != this.mx.GetLength(1) - 1)
            {
              sw.Write(" ");
            }
          }
          sw.WriteLine();
        }
      }
    }
  }
}
