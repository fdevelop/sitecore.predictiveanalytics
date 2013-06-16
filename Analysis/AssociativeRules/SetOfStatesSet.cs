using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.PredictivePersonalization.Analysis.AssociativeRules
{
  public class SetOfStatesSet : IEnumerable<StatesSet>
  {
    public SetOfStatesSet()
    {
      this.internalSet = new HashSet<StatesSet>();
      this.internalIgnoreSet = new HashSet<StatesSet>();
      this.singleCache = new HashSet<int>();
    }

    public void Add(StatesSet e)
    {
      this.internalSet.Add(e);
      foreach (var ix in e)
      {
        this.singleCache.Add(ix);
      }
    }

    public void AddRange(IEnumerable<StatesSet> c)
    {
      foreach (var e in c)
      {
        this.internalSet.Add(e);
      }
    }

    public void AddIgnore(StatesSet e)
    {
      this.internalIgnoreSet.Add(e);
    }

    public ISet<StatesSet> InnerSet
    {
      get
      {
        return this.internalSet;
      }
    }

    public ISet<StatesSet> InnerIgnoreSet
    {
      get
      {
        return this.internalIgnoreSet;
      }
    }

    public void MoveToIgnore(StatesSet ss)
    {
      this.internalSet.Remove(ss);
      this.internalIgnoreSet.Add(ss);
    }

    public IEnumerable<int> GetSingleSet()
    {
      return this.singleCache.AsEnumerable().OrderBy(k => k);
    }

    public IEnumerator<StatesSet> GetEnumerator()
    {
      return this.internalSet.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.internalSet.GetEnumerator();
    }

    private ISet<StatesSet> internalSet;
    private ISet<StatesSet> internalIgnoreSet;
    private HashSet<int> singleCache;
  }
}
