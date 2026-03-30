namespace CTScope.Dicom.Models;

public class DicomFolderScanResult
{
    public string RootFolderPath { get; set; } = string.Empty;

    public int TotalFilesScanned { get; set; }

    public int DicomFilesOpened { get; set; }

    public List<DicomStudyInfo> Studies { get; set; } = new();

    public List<string> Messages { get; set; } = new();
}
