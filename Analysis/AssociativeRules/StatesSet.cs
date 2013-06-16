using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.PredictivePersonalization.Analysis.AssociativeRules
{
  public class StatesSet : IEnumerable<int>, ICloneable
  {
    public StatesSet(int[] vals)
    {
      this.values = new HashSet<int>(vals);
    }

    public static StatesSet GetSet(params int[] values)
    {
      return new StatesSet(values);
    }

    public StatesSet Add(int v)
    {
      this.values.Add(v);
      return this;
    }

    public int Power { get; set; }

    public int Size
    {
      get
      {
        return this.values.Count;
      }
    }

    public ISet<int> InnerSet
    {
      get
      {
        return this.values;
      }
    }

    public IEnumerator<int> GetEnumerator()
    {
      return this.values.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.values.GetEnumerator();
    }

    public object Clone()
    {
      return new StatesSet(this.values.ToArray())
      {
        Power = this.Power
      };
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      var arr = this.values.ToArray();
      for (int i = 0; i < arr.Length; i++)
      {
        if (i > 0)
        {
          sb.Append(", ");
        }

        sb.Append(arr[i]);
      }

      return "{" + sb.ToString() + "}";
    }

    public bool ContentEquals(object obj)
    {
      if (obj is StatesSet)
      {
        ISet<int> one = (obj as StatesSet).values;
        ISet<int> two = this.values;

        if (one.All(c => two.Contains(c)))
        {
          return true;
        }
      }

      return false;
    }

    private ISet<int> values;
  }
}
