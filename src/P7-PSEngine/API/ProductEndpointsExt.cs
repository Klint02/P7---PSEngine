using Microsoft.AspNetCore.Mvc;
using P7_PSEngine.BackgroundServices;
using P7_PSEngine.Model;
using P7_PSEngine.Services;

namespace P7_PSEngine.API
{
    public static class ProductEndpointsExt
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            app.MapGet("/api/products", async (ITodoRepository repo) =>
            {
                var products = await repo.GetAllTodosAsync();
                return Results.Ok(products);
            });

            app.MapGet("/api/products/{id}", async (HttpContext context, ITodoRepository repo) =>
            {
                if (!int.TryParse(context.Request.RouteValues["id"]?.ToString(), out var id))
                {
                    return Results.BadRequest("Invalid product ID");
                }

                var product = await repo.GetTodoByIdAsync(id);
                if (product == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(product);
            });

            app.MapGet("/todoitems/{id}", async (int id, ITodoRepository repo) =>
                await repo.GetTodoByIdAsync(id)
                    is Todo todo
                        ? Results.Ok(todo)
                        : Results.NotFound());


            // Add the endpoint for IndexController
            app.MapGet("/api/index", async ([FromBody] SessionCookieDTO userDTO, [FromServices] IInvertedIndexService invertedIndexService, [FromServices] IUserRepository userRepository) =>
            {
                User? user = await userRepository.GetUserByUsernameAsync(userDTO.username);
                if (user == null)
                {
                    return Results.BadRequest("User not found");
                }
                await invertedIndexService.InitializeUser(user);
                return Results.Ok();
            });

            // Add the endpoint for SearchController
            app.MapPost("/api/search", async ([FromBody] SearchRequestDTO searchRequest, [FromServices] ISearchService searchService, [FromServices] IUserRepository userRepository) =>
            {

                // Check if the search details are empty
                if (searchRequest.SearchDetails == null || string.IsNullOrEmpty(searchRequest.SearchDetails.searchwords))
                {
                    return Results.BadRequest("Search details cannot be empty");
                }

                // Get the user from the session cookie
                User user = await userRepository.GetUserByUsernameAsync(searchRequest.SessionCookie.username);
                if (user == null)
                {
                    return Results.BadRequest("User not found");
                }

                // Call boolsearch for the entire search query
                var searchResult = await searchService.BoolSearch(searchRequest.SearchDetails.searchwords, user);
                if (searchResult.TotalResults == 0)
                {
                    return Results.NotFound("No valid search terms found");
                }
                return Results.Ok(searchResult);
            });


            app.MapGet("/api/messages", (SampleData data, string q = "") => data.Data);

        }
    }

    public class SearchRequestDTO
    {
        public SessionCookieDTO SessionCookie { get; set; }
        public SearchDetailsDTO SearchDetails { get; set; }
    }
}
