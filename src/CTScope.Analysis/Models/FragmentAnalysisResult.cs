namespace CTScope.Analysis.Models;

public class FragmentAnalysisResult
{
    public List<FragmentInfo> Fragments { get; set; } = new();

    public string Message { get; set; } = string.Empty;
}
