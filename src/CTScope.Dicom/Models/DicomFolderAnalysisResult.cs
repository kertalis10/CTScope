namespace CTScope.Dicom.Models;

public class DicomFolderAnalysisResult
{
    public string SourceFolder { get; set; } = string.Empty;

    public List<string> SeriesIds { get; set; } = new();

    public List<string> Messages { get; set; } = new();
}
