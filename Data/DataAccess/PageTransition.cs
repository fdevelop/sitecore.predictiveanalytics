namespace Sitecore.PredictivePersonalization.Data.DataAccess
{
  using System;

  public class PageTransition
  {
    public long PageFrom { get; set; }
    public long PageTo { get; set; }
    public TimeSpan DelayBetweenStates { get; set; }

    public PageTransition(long from, long to, TimeSpan delay)
    {
      this.PageFrom = from;
      this.PageTo = to;

      this.DelayBetweenStates = delay;
    }
  }
}
