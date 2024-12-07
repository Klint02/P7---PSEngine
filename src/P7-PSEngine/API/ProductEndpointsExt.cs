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
                User user = await userRepository.GetUserByUsernameAsync(userDTO.username);
                invertedIndexService.InitializeUser(user);
                return Results.Ok();
            });

            // Add the endpoint for SearchController
            app.MapPost("/api/search", async ([FromBody] SearchRequestDTO searchRequest, [FromServices] ISearchService searchService, [FromServices] IUserRepository userRepository ) =>
            {
                User user = await userRepository.GetUserByUsernameAsync(searchRequest.SessionCookie.username);
                if (searchRequest.SearchDetails == null || string.IsNullOrEmpty(searchRequest.SearchDetails.searchwords))
                {
                    return Results.BadRequest("Search details cannot be empty");
                }

                IEnumerable<string> searchQueries = await searchService.ProcessSearchQuery(searchRequest.SearchDetails.searchwords);
                if (!searchQueries.Any())
                {
                    return Results.BadRequest("Invalid search term");
                }
                
                foreach (var query in searchQueries)
                {
                    if (string.IsNullOrEmpty(query))
                    {
                        return Results.BadRequest("Invalid search term");
                    }
                    var searchResult = await searchService.BoolSearch(query, user);
                    Console.WriteLine($"Search result for {query}: {searchResult}");
                    return Results.Ok(searchResult);
                }
                return Results.BadRequest("No valid search terms found"); 
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
