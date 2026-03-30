using CTScope.Dicom.Readers;

namespace CTScope.Tests;

public class DicomStudyReaderTests
{
    [Fact]
    public void ScanFolder_InvalidPath_ThrowsDirectoryNotFoundException()
    {
        var reader = new DicomStudyReader();
        var missingPath = Path.Combine(Path.GetTempPath(), $"ctscope-missing-{Guid.NewGuid():N}");

        Assert.Throws<DirectoryNotFoundException>(() => reader.ScanFolder(missingPath));
    }

    [Fact]
    public void ScanFolder_EmptyFolder_ReturnsNoStudies()
    {
        var reader = new DicomStudyReader();
        var folderPath = Directory.CreateTempSubdirectory("ctscope-empty-").FullName;

        try
        {
            var result = reader.ScanFolder(folderPath);

            Assert.Equal(folderPath, result.RootFolderPath);
            Assert.Equal(0, result.TotalFilesScanned);
            Assert.Equal(0, result.DicomFilesOpened);
            Assert.Empty(result.Studies);
        }
        finally
        {
            Directory.Delete(folderPath, recursive: true);
        }
    }
}
