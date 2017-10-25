namespace SpellCorrector.Web.Infrastructure
{
    public interface ILevenshteinDistanceAlgorithm
    {
        int GetDistance(string first, string second);
    }
}
