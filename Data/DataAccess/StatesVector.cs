namespace Sitecore.PredictivePersonalization.Data.DataAccess
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public class StatesVector : IEnumerable<KeyValuePair<string, double>>
  {
    private KeyValuePair<string, double>[] values;

    public StatesVector(IEnumerable<KeyValuePair<string, double>> v)
    {
      this.values = v.ToArray();
    }

    public int Length
    {
      get
      {
        return this.values.Length;
      }
    }

    public KeyValuePair<string, double> this[int idx]
    {
      get
      {
        return this.values[idx];
      }
      set
      {
        this.values[idx] = value;
      }
    }

    public IEnumerator<KeyValuePair<string, double>> GetEnumerator()
    {
      return this.values.AsEnumerable().GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.values.GetEnumerator();
    }
  }
}
