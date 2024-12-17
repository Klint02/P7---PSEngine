// Adapter for Google Drive files
public class GoogleDriveFile : ICloudFile
{
    private readonly FileData _file;

    public GoogleDriveFile(FileData file)
    {
        _file = file;
    }

    public string Id => _file.Id;
    public string Name => _file.Name;
}

public class FileData
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class FileList
{
    public string NextPageToken { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public bool IncompleteSearch { get; set; } = false;
    public List<FileData> Files { get; set; } = new List<FileData>();
}