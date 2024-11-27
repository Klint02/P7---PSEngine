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