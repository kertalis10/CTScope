namespace CTScope.Dicom.Models;

public class DicomVolumeData
{
    public int Width { get; set; }

    public int Height { get; set; }

    public int Depth { get; set; }

    public float[,,]? DensityMatrix { get; set; }

    public double SpacingX { get; set; }

    public double SpacingY { get; set; }

    public double SpacingZ { get; set; }

    public string SeriesId { get; set; } = string.Empty;
}
