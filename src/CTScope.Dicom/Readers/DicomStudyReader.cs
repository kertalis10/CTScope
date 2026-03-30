using CTScope.Dicom.Models;
using FellowOakDicom;

namespace CTScope.Dicom.Readers;

public class DicomStudyReader
{
    public DicomFolderAnalysisResult AnalyzeFolder(string folderPath)
    {
        var scanResult = ScanFolder(folderPath);

        return new DicomFolderAnalysisResult
        {
            SourceFolder = scanResult.RootFolderPath,
            SeriesIds = scanResult.Studies
                .SelectMany(study => study.Series)
                .Select(series => series.SeriesInstanceUid)
                .ToList(),
            Messages = scanResult.Messages
        };
    }

    public DicomFolderScanResult ScanFolder(string folderPath)
    {
        if (string.IsNullOrWhiteSpace(folderPath))
        {
            throw new ArgumentException("Folder path must not be empty.", nameof(folderPath));
        }

        if (!Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"The folder '{folderPath}' does not exist.");
        }

        var result = new DicomFolderScanResult { RootFolderPath = folderPath };
        var studiesByUid = new Dictionary<string, DicomStudyInfo>(StringComparer.Ordinal);

        foreach (var filePath in Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories))
        {
            result.TotalFilesScanned++;

            var appearsDicom = DicomFile.HasValidHeader(filePath);
            if (!appearsDicom)
            {
                continue;
            }

            try
            {
                var dicomFile = DicomFile.Open(filePath);
                var dataset = dicomFile.Dataset;

                var studyUid = GetOrFallback(dataset, DicomTag.StudyInstanceUID, "UNKNOWN-STUDY");
                var seriesUid = GetOrFallback(dataset, DicomTag.SeriesInstanceUID, "UNKNOWN-SERIES");
                var hasPixelData = dataset.Contains(DicomTag.PixelData);

                var fileInfo = new DicomFileInfo
                {
                    FilePath = filePath,
                    FileName = Path.GetFileName(filePath),
                    SopInstanceUid = TryGetString(dataset, DicomTag.SOPInstanceUID),
                    InstanceNumber = TryGetInt(dataset, DicomTag.InstanceNumber),
                    ImagePositionPatientRaw = TryGetJoinedValues(dataset, DicomTag.ImagePositionPatient),
                    HasPixelData = hasPixelData
                };

                if (!studiesByUid.TryGetValue(studyUid, out var study))
                {
                    study = new DicomStudyInfo
                    {
                        StudyInstanceUid = studyUid,
                        StudyDescription = TryGetString(dataset, DicomTag.StudyDescription),
                        StudyDate = TryGetString(dataset, DicomTag.StudyDate),
                        PatientName = TryGetString(dataset, DicomTag.PatientName),
                        PatientId = TryGetString(dataset, DicomTag.PatientID)
                    };

                    studiesByUid.Add(studyUid, study);
                }

                var series = study.Series.FirstOrDefault(s => s.SeriesInstanceUid == seriesUid);
                if (series is null)
                {
                    var spacing = TryGetDoubleArray(dataset, DicomTag.PixelSpacing);

                    series = new DicomSeriesInfo
                    {
                        SeriesInstanceUid = seriesUid,
                        SeriesDescription = TryGetString(dataset, DicomTag.SeriesDescription),
                        SeriesNumber = TryGetInt(dataset, DicomTag.SeriesNumber),
                        Modality = TryGetString(dataset, DicomTag.Modality),
                        Rows = TryGetInt(dataset, DicomTag.Rows),
                        Columns = TryGetInt(dataset, DicomTag.Columns),
                        SpacingX = spacing.ElementAtOrDefault(0),
                        SpacingY = spacing.ElementAtOrDefault(1),
                        SliceThickness = TryGetDouble(dataset, DicomTag.SliceThickness)
                    };

                    study.Series.Add(series);
                }

                series.Files.Add(fileInfo);
                series.FileCount = series.Files.Count;
                series.HasPixelData = series.HasPixelData || hasPixelData;

                result.DicomFilesOpened++;
            }
            catch (Exception ex)
            {
                result.Messages.Add($"Skipped unreadable DICOM file: {filePath} ({ex.Message})");
            }
        }

        result.Studies = studiesByUid.Values
            .OrderBy(study => study.StudyDate)
            .ThenBy(study => study.StudyDescription)
            .ToList();

        foreach (var study in result.Studies)
        {
            study.Series = study.Series
                .OrderBy(series => series.SeriesNumber ?? int.MaxValue)
                .ThenBy(series => series.SeriesDescription)
                .ToList();
        }

        return result;
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

    private static string GetOrFallback(DicomDataset dataset, DicomTag tag, string fallback)
    {
        var value = TryGetString(dataset, tag);
        return string.IsNullOrWhiteSpace(value) ? fallback : value;
    }

    private static string? TryGetString(DicomDataset dataset, DicomTag tag)
    {
        return dataset.TryGetSingleValue(tag, out string? value) ? value : null;
    }

    private static int? TryGetInt(DicomDataset dataset, DicomTag tag)
    {
        return dataset.TryGetSingleValue(tag, out int value) ? value : null;
    }

    private static double? TryGetDouble(DicomDataset dataset, DicomTag tag)
    {
        return dataset.TryGetSingleValue(tag, out double value) ? value : null;
    }

    private static double?[] TryGetDoubleArray(DicomDataset dataset, DicomTag tag)
    {
        if (!dataset.TryGetValues(tag, out double[]? values) || values is null)
        {
            return [];
        }

        return values.Cast<double?>().ToArray();
    }

    private static string? TryGetJoinedValues(DicomDataset dataset, DicomTag tag)
    {
        if (!dataset.TryGetValues(tag, out string[]? values) || values is null)
        {
            return null;
        }

        return string.Join("\\", values);
    }
}
