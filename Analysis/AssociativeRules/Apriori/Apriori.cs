using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.PredictivePersonalization.Analysis.AssociativeRules.Apriori
{
  public class Apriori : IAssociativeRulesAlgorithm
  {
    public Apriori()
    {
    }

    public Apriori(int[,] sessions)
    {
      this.Initialize(sessions);
    }

    public void Initialize(int[,] mx)
    {
      this.sessions = (int[,])mx.Clone();
    }

    public SetOfStatesSet FindFrequentSets(int minSupport)
    {
      var set = new SetOfStatesSet();
      var setForIgnore = new HashSet<StatesSet>();

      var f1 = FindFrequentSingleSets(minSupport);
      set.AddRange(f1.InnerSet);

      while (f1.InnerSet.Count > 0)
      {
        var ck = this.AprioriGen(f1);
        foreach (var item in ck)
        {
          for (int i = 0; i < this.sessions.GetLength(0); i++)
          {
            // check session
            bool isOk = true;
            foreach (var symbol in item)
            {
              if (this.sessions[i, symbol] == 0)
              {
                isOk = false;
                break;
              }
            }

            if (isOk)
            {
              item.Power = item.Power + 1;
            }
          }

          if (item.Power < minSupport)
          {
            setForIgnore.Add(item);
          }
        }

        foreach (var item in setForIgnore)
        {
          ck.MoveToIgnore(item);
        }

        f1 = ck;
        set.AddRange(f1.InnerSet);
      }

      return set;
    }

    public IEnumerable<AssociativeRule> GenerateRules(SetOfStatesSet statesSet, double minConfidence)
    {
      foreach (StatesSet item in statesSet)
      {
        if (item.Size < 2)
        {
          continue;
        }

        var rulesCandidates = this.GetPossibleRules(statesSet, item);
        foreach (var candidate in rulesCandidates)
        {
          double conf = (double)item.Power / (double)candidate.Item1.Power;
          if (conf > minConfidence)
          {
            yield return new AssociativeRule(candidate.Item1, candidate.Item2, item.Power, conf);
          }
        }
      }
    }

    private IEnumerable<Tuple<StatesSet, StatesSet>> GetPossibleRules(SetOfStatesSet statesSet, StatesSet source)
    {
      foreach (var item in statesSet)
      {
        if (item.InnerSet.IsSubsetOf(source.InnerSet) && item != source)
        {
          yield return new Tuple<StatesSet, StatesSet>((StatesSet)item.Clone(), new StatesSet(source.InnerSet.Except(item.InnerSet).ToArray()));
        }
      }
    }

    private SetOfStatesSet AprioriGen(SetOfStatesSet f)
    {
      Contract.Assert(f.InnerSet.Any(), "Input set should not be empty!");
      int newSize = ((StatesSet)f.InnerSet.First()).Size + 1;
      int[] unique = f.GetSingleSet().ToArray();

      SetOfStatesSet result = new SetOfStatesSet();

      if (unique.Length < newSize)
      {
        return result;
      }

      for (int k = 0; k < unique.Length; k++)
      {
        this.AprioriGenReccur(f, result, StatesSet.GetSet(unique[k]), k, 0, newSize, unique);
      }

      return result;
    }

    private void AprioriGenReccur(SetOfStatesSet source, SetOfStatesSet result, StatesSet current, int startWithUniqueIndex, int index, int newSize, int[] unique)
    {
      if (index < newSize - 2)
      {
        if (unique.Length > startWithUniqueIndex + 1)
        {
          this.AprioriGenReccur(source, result, current.Add(unique[startWithUniqueIndex + 1]), startWithUniqueIndex + 1, index + 1, newSize, unique);
        }
      }
      else if (index == newSize - 2)
      {
        for (int k = startWithUniqueIndex + 1; k < unique.Length; k++)
        {
          var src = current.Clone() as StatesSet;
          src.Add(unique[k]);

          var allow = true;
          foreach (var black in source.InnerIgnoreSet)
          {
            if (black.InnerSet.IsSubsetOf(src))
            {
              allow = false;
              break;
            }
          }

          if (allow)
          {
            result.Add(src);
          }
          else
          {
            result.AddIgnore(src);
          }
        }
      }
    }

    protected SetOfStatesSet FindFrequentSingleSets(int minSupport)
    {
      var set = new SetOfStatesSet();

      int[] counts = new int[this.sessions.GetLength(1)];
      for (int i = 0; i < this.sessions.GetLength(0); i++)
      {
        for (int j = 0; j < this.sessions.GetLength(1); j++)
        {
          counts[j] += this.sessions[i,j];
        }
      }

      for (int k = 0; k < this.sessions.GetLength(1); k++)
      {
        var s = StatesSet.GetSet(k);
        s.Power = counts[k];
        if (counts[k] >= minSupport)
        {
          set.Add(s);
        }
        else
        {
          set.AddIgnore(s);
        }
      }

      return set;
    }

    private int[,] sessions;
  }
}
