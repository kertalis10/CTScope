namespace CTScope.Dicom.Models;

public class DicomStudyInfo
{
    public string StudyInstanceUid { get; set; } = string.Empty;

    public string? StudyDescription { get; set; }

    public string? StudyDate { get; set; }

    public string? PatientName { get; set; }

    public string? PatientId { get; set; }

    public List<DicomSeriesInfo> Series { get; set; } = new();

    public string DisplayName => string.IsNullOrWhiteSpace(StudyDescription)
        ? $"Study {StudyInstanceUid}"
        : StudyDescription;
}
