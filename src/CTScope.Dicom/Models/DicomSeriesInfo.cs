namespace CTScope.Dicom.Models;

public class DicomSeriesInfo
{
    public string SeriesInstanceUid { get; set; } = string.Empty;

    public string? SeriesDescription { get; set; }

    public int? SeriesNumber { get; set; }

    public string? Modality { get; set; }

    public int FileCount { get; set; }

    public int? Rows { get; set; }

    public int? Columns { get; set; }

    public double? SpacingX { get; set; }

    public double? SpacingY { get; set; }

    public double? SliceThickness { get; set; }

    public bool HasPixelData { get; set; }

    public List<DicomFileInfo> Files { get; set; } = new();

    public string DisplayName
    {
        get
        {
            var numberPart = SeriesNumber.HasValue ? $"[{SeriesNumber}] " : string.Empty;
            var descriptionPart = string.IsNullOrWhiteSpace(SeriesDescription) ? "Unnamed series" : SeriesDescription;
            var modalityPart = string.IsNullOrWhiteSpace(Modality) ? "Unknown" : Modality;
            return $"{numberPart}{descriptionPart} ({modalityPart}, {FileCount} files)";
        }
    }
}
