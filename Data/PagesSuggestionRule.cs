using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sitecore.PredictivePersonalization.Data
{
  public class PagesSuggestionRule
  {
    public IEnumerable<string> RequiredPages { get; set; }
    public IEnumerable<string> SuggestedPages { get; set; }

    public double RequiredPagesPercentage { get; set; }
    public double RuleSuccessPercentage { get; set; }
    public double RuleConfidence { get; set; }
  }
}
