namespace CTScope.Dicom.Models;

public class DicomFileInfo
{
    public string FilePath { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string? SopInstanceUid { get; set; }

    public int? InstanceNumber { get; set; }

    public string? ImagePositionPatientRaw { get; set; }

    public bool HasPixelData { get; set; }
}
