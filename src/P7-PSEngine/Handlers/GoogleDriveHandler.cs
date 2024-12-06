using P7_PSEngine.Model;
using P7_PSEngine.API;
using P7_PSEngine.DTO;
namespace P7_PSEngine.Handlers;


public class GoogleDriveHandler : ICloudServiceHandler 
{
    private readonly IFileInformationRepository _file_repo;

    public GoogleDriveHandler(IFileInformationRepository file_repo) 
    {
        _file_repo = file_repo;
    }

    public async Task<bool> OAuth2Handler(WebApplication app, ServiceCreationDetailsDTO service) 
    {
        return true;
    }

    public async Task<List<FileInformation>> ServiceFileRequest(WebApplication app, User user, string? path, CloudService service) {
        return new List<FileInformation>();
        
    }
}
