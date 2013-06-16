using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.PredictivePersonalization.Analysis.AssociativeRules
{
  public class AssociativeRule
  {
    public StatesSet Condition { get; set; }
    public StatesSet Outcome { get; set; }

    public double ConditionSuccess { get; set; }
    
    public double Confidence { get; set; }

    public AssociativeRule(StatesSet condition, StatesSet outcome, double conditionSuccess, double conf)
    {
      this.Condition = (StatesSet)condition.Clone();
      this.Outcome = (StatesSet)outcome.Clone();

      this.ConditionSuccess = conditionSuccess;
      this.Confidence = conf;
    }

    public AssociativeRule(StatesSet condition, int outcome, double conditionSuccess, double conf)
      : this(condition, new StatesSet(new int[] { outcome }), conditionSuccess, conf)
    {
    }

    public override string ToString()
    {
      return this.Condition + " => " + this.Outcome + " (" + this.Confidence.ToString("F3") + ")";
    }
  }
}
