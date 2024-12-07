using System.Threading.Tasks;
using P7_PSEngine.Model;
using P7_PSEngine.DTO;
using P7_PSEngine.Services;

namespace P7_PSEngine.Handlers;

public interface ICloudServiceHandler 
{
    //TODO: (nkc) all oauth handler implementations should implement check already used service names
    public Task<bool> OAuth2Handler(WebApplication app, ServiceCreationDetailsDTO service);

    public Task<List<FileInformation>> ServiceFileRequest(WebApplication app, User user, string? path, CloudService service);

    public Task<bool> ProcessFiles(WebApplication app, User user, string? path, CloudService service, IInvertedIndexService index);
}