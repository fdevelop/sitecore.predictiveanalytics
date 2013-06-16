namespace Sitecore.PredictivePersonalization.Data.DataAccess
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.IO;
  using System.Text;

  /// <summary>
  ///     This class represents the object to store the input matrix for cluster analyze methods.<br />
  ///     DataMatrix is a matrix, where each line is a point for analyze.
  /// </summary>
  public class DataMatrix
  {
    private double[][] data;

    private double[][] ReadDataFile(string fileName)
    {
      List<double[]> result = new List<double[]>();
      StreamReader sr = new StreamReader(fileName);
      while (!sr.EndOfStream)
      {
        List<double> lineVals = new List<double>();
        foreach (string s in sr.ReadLine().Split(' '))
        {
          if (s.Trim().Length > 0)
          {
            try
            {
              lineVals.Add(Double.Parse(s));
            }
            catch
            {
              lineVals.Add(0);
            }
          }
        }
        result.Add(lineVals.ToArray());
      }

      return result.ToArray();
    }

    /// <summary>
    /// create the new matrix from a file of n lines, each contains m whitespace-separated numbers.
    /// </summary>
    /// <param name="fileName">path to the local file</param>
    public DataMatrix(string fileName)
    {
      data = ReadDataFile(fileName);
    }

    /// <summary>
    /// create the new matrix from the specified array
    /// </summary>
    /// <param name="pdata">data to assign</param>
    public DataMatrix(double[][] pdata)
    {
      data = new double[pdata.Length][];
      for (int i = 0; i < pdata.Length; i++)
      {
        data[i] = new double[pdata[i].Length];
        for (int j = 0; j < pdata[i].Length; j++)
        {
          data[i][j] = pdata[i][j];
        }
      }
    }

    /// <summary>
    /// modify matrix's elements by formula: (M[i][j] - Average(j)) / Dispersion(j)
    /// </summary>
    public void Standartize()
    {
      for (int j = 0; j < ColumnsCount; j++)
      {
        double average = 0;
        for (int i = 0; i < data.Length; i++)
        {
          average += data[i][j];
        }
        average /= data.Length;

        double dispersion = 0;
        for (int i = 0; i < data.Length; i++)
        {
          dispersion += System.Math.Pow(data[i][j] - average, 2);
        }
        dispersion /= (data.Length - 1);

        for (int i = 0; i < data.Length; i++)
        {
          data[i][j] = (data[i][j] - average) / dispersion;
        }
      }
    }

    /// <summary>
    /// returns data matrix
    /// </summary>
    public double[][] Data
    {
      get { return data; }
    }

    /// <summary>
    /// returns count of matrix's columns
    /// </summary>
    public int ColumnsCount
    {
      get { return (data.Length > 0) ? data[0].Length : 0; }
    }

    /// <summary>
    /// returns count of matrix's rows
    /// </summary>
    public int RowsCount
    {
      get { return (data.Length); }
    }
  }
}
