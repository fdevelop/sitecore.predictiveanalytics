namespace Sitecore.PredictivePersonalization.Data.DataAccess
{
  public interface ITrailsBuilderService
  {
    string[] GetStateNames();
    VisitTrail[] GetTrails();
  }
}
