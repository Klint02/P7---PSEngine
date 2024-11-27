using System.Collections.Generic;
using Newtonsoft.Json;
using CloudSearcher;


public class CloudFileLoader
{
    public IEnumerable<ICloudFile> LoadGoogleDriveFiles(string json)
    {
        var fileList = JsonConvert.DeserializeObject<FileList>(json);
        return fileList.Files.Select(file => new GoogleDriveFile(file));
    }

    public IEnumerable<ICloudFile> LoadDropboxFiles(string json)
    {
        var folderResponse = JsonConvert.DeserializeObject<FolderResponse>(json);
        return folderResponse.Entries.Select(entry => new DropboxFile(entry));
    }
}
