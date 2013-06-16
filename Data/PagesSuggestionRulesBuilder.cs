using Sitecore.Diagnostics;
using Sitecore.PredictivePersonalization.Analysis.AssociativeRules;
using Sitecore.PredictivePersonalization.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sitecore.PredictivePersonalization.Data
{
  public class PagesSuggestionRulesBuilder
  {
    public PagesSuggestionRulesBuilder(IAssociativeRulesAlgorithm engine)
    {
      this.engine = engine;
    }

    public IEnumerable<PagesSuggestionRule> GetRules(PageVisitsMatrix mx, int minSupport, double minConfidence)
    {
      Log.Debug("Building of associative rules for Pages started.");

      int[,] intMatrix = new int[mx.Matrix.GetLength(0), mx.Matrix.GetLength(1)];
      for (int i = 0; i < mx.Matrix.GetLength(0); i++)
      {
        for (int j = 0; j < mx.Matrix.GetLength(1); j++)
        {
          intMatrix.SetValue((int)mx.Matrix[i, j], i, j);
        }
      }

      this.engine.Initialize(intMatrix);
      SetOfStatesSet result = this.engine.FindFrequentSets(minSupport);

      var rules = this.engine.GenerateRules(result, minConfidence).ToList();

      var suggestions = new List<PagesSuggestionRule>();
      foreach (var item in rules)
      {
        Log.Debug("Building of associative rules: " + item);
        var suggestionItem = new PagesSuggestionRule()
        {
          RequiredPages = item.Condition.Select(num => mx.StateNames[num]),
          SuggestedPages = item.Outcome.Select(num => mx.StateNames[num]),
          RuleConfidence = item.Confidence,
          RuleSuccessPercentage = 100.0 * (double)item.ConditionSuccess / (double)mx.Matrix.GetLength(0),
          RequiredPagesPercentage = 100.0 * (double)item.Condition.Power / (double)mx.Matrix.GetLength(0)
        };
        suggestions.Add(suggestionItem);
      }

      Log.Debug("Building of associative rules for Pages finished.");

      return suggestions;
    }

    private IAssociativeRulesAlgorithm engine;
  }
}
