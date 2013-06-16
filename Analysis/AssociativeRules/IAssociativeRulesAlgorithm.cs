using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sitecore.PredictivePersonalization.Analysis.AssociativeRules
{
  public interface IAssociativeRulesAlgorithm
  {
    void Initialize(int[,] mx);
    SetOfStatesSet FindFrequentSets(int minSupport);
    IEnumerable<AssociativeRule> GenerateRules(SetOfStatesSet statesSet, double minConfidence);
  }
}
