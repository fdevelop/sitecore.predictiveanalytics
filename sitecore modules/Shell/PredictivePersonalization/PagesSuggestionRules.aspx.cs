using Sitecore.PredictivePersonalization.Analysis.AssociativeRules;
using Sitecore.PredictivePersonalization.Analysis.AssociativeRules.Apriori;
using Sitecore.PredictivePersonalization.Data;
using Sitecore.PredictivePersonalization.Data.DataAccess;
using Sitecore.PredictivePersonalization.Data.DataAccess.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.PredictivePersonalization.View
{
  public partial class PagesSuggestionRules : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      int minSupport = 10;
      int.TryParse(Request.QueryString["minSupport"], out minSupport);

      int minPagesCondition = 2;
      int.TryParse(Request.QueryString["minPagesCondition"], out minPagesCondition);

      double minConfidence = 0.5;
      double.TryParse(Request.QueryString["minConfidence"], out minConfidence);

      ITrailsBuilderService service = new SqlTrailsBuilder();
      PageVisitsMatrix pvm = new PageVisitsMatrix(service);

      IAssociativeRulesAlgorithm associativeRulesAlgo = new Apriori();
      PagesSuggestionRulesBuilder rulesBuilder = new PagesSuggestionRulesBuilder(associativeRulesAlgo);
      var rulesBuilt = rulesBuilder.GetRules(pvm, minSupport, minConfidence);

      this.gridRules.DataSource = from r in rulesBuilt
                                  where r.RequiredPages.Count() > minPagesCondition
                                  orderby r.RequiredPagesPercentage descending
                                  select new
                                  {
                                    RequiredPages = string.Join(",\r\n", r.RequiredPages),
                                    SuggestedPages = string.Join(",\r\n", r.SuggestedPages),
                                    RequiredPagesPercentage = r.RequiredPagesPercentage.ToString("F3") + "%",
                                    RuleSuccessPercentage = r.RuleSuccessPercentage.ToString("F3") + "%",
                                    RuleConfidence = (r.RuleConfidence * 100).ToString("F0") + "%"
                                  };

      this.gridRules.DataBind();
    }
  }
}