namespace Sitecore.PredictivePersonalization.Data.DataAccess
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public class VisitTrail
  {
    private long[] seq;
    private TimeSpan[] delays;

    public VisitTrail(long[] seq)
    {
      this.seq = (long[])seq.Clone();
      this.delays = new TimeSpan[this.seq.Length - 1];

      for (int i = 0; i < this.delays.Length; i++)
      {
        this.delays[i] = new TimeSpan(0, 0, 0);
      }
    }

    public VisitTrail(long[] seq, TimeSpan[] delays)
    {
      this.seq = (long[])seq.Clone();
      this.delays = (TimeSpan[])delays.Clone();
    }

    public long this[long idx]
    {
      get
      {
        return this.seq[idx];
      }
    }

    public long PageCount
    {
      get
      {
        return this.seq.Length;
      }
    }

    public IEnumerable<PageTransition> GetTransitions()
    {
      for (int i = 1; i < this.seq.Length; i++)
      {
        yield return new PageTransition(this.seq[i-1], this.seq[i], this.delays[i-1]);
      }
    }

    public IEnumerable<long> GetPages()
    {
      for (int i = 0; i < this.seq.Length; i++)
      {
        yield return this.seq[i];
      }
    }
  }
}
