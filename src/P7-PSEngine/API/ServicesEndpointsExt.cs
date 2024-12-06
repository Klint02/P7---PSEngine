using P7_PSEngine.DTO;
using P7_PSEngine.Data;
using P7_PSEngine.Model;
using P7_PSEngine.Handlers;

namespace P7_PSEngine.API
{
    public static class ServicesEndpointsExt
    {
        public static void MapServicesEndpoints(this WebApplication app)
        {
            string static_path = "/app/wwwroot";

            app.MapGet("/linkuser", () => Results.Content(File.ReadAllText($"{static_path}/html/auth.html"), "text/html"));

            app.MapPost("/api/addservice", async (ServiceCreationDetailsDTO Details, IFileInformationRepository file_repo, IUserRepository user_repo, ICloudServiceRepository cloud_repo) => {
                ICloudServiceHandler service_handler;

                if (Details.service_type == "dropbox") {
                    service_handler = new DropBoxHandler(file_repo, user_repo, cloud_repo);
                } else {
                    service_handler = new GoogleDriveHandler(file_repo);
                }
                User? user = user_repo.GetUserByUsernameAsync(Details.user).Result;
                CloudService? service_check = cloud_repo.GetServiceByDefinedNameAsync(Details.user_defined_service_name).Result;
                if (service_check != null) {
                    Details.user_defined_service_name = Details.user_defined_service_name + DateTime.Now.ToString(); 
                }

                var oauth2_task_result = await service_handler.OAuth2Handler(app, Details);

                CloudService? service = cloud_repo.GetServiceByDefinedNameAsync(Details.user_defined_service_name).Result;
                if (service != null && user != null) {
                    await service_handler.ServiceFileRequest(app, user, null, service);

                }
                if (oauth2_task_result) {
                    return new DataErrorDTO{Data = "Service created succesfully", Error = ""};

                } else {
                    return new DataErrorDTO{Data = "", Error = "Failed to create service"};

                }
            });

            app.MapPost("/api/fetchservices", (SessionCookieDTO user) => {
                
            });

            app.MapPost("/api/deleteservice/{id}", (SessionCookieDTO user, int id) => {
                
            });

        }
    }
}
