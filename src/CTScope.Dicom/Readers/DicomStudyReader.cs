using CTScope.Dicom.Models;

namespace CTScope.Dicom.Readers;

public class DicomStudyReader
{
    public DicomFolderAnalysisResult AnalyzeFolder(string folderPath)
    {
        return new DicomFolderAnalysisResult
        {
            SourceFolder = folderPath,
            Messages = new List<string>
            {
                "Folder analysis is not implemented yet."
            }
        };
    }

    public DicomVolumeData LoadVolume(string folderPath, string seriesId)
    {
        return new DicomVolumeData
        {
            SeriesId = seriesId,
            DensityMatrix = null,
            Width = 0,
            Height = 0,
            Depth = 0,
            SpacingX = 0,
            SpacingY = 0,
            SpacingZ = 0
        };
    }
}
