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

            //app.MapPost("/api/products", async (Todo todos, PSengineDB db) =>
            //{
            //    db.Todos.Add(todos);
            //    await db.SaveChangesAsync();
            //    return Results.Created($"/api/products/{todos.Id}", todos);
            //});

            // app.MapPut("/api/products/{id}", async (int id, Todo todo, PSengineDB db) =>
            // {
            //     if (id != todo.Id)
            //     {
            //         return Results.BadRequest();
            //     }

            //     db.Entry(todo).State = EntityState.Modified;

            //     try
            //     {
            //         await db.SaveChangesAsync();
            //     }
            //     catch (DbUpdateConcurrencyException)
            //     {
            //         if (await db.Todos.FindAsync(id) == null)
            //         {
            //             return Results.NotFound();
            //         }
            //         else
            //         {
            //             throw;
            //         }
            //     }

            //     return Results.NoContent();
            // });

            //app.MapDelete("/api/products/{id}", async (int id, PSengineDB db) =>
            //{
            //    var product = await db.Todos.FindAsync(id);
            //    if (product == null)
            //    {
            //        return Results.NotFound();
            //    }

            //    db.Todos.Remove(product);
            //    await db.SaveChangesAsync();

            //    return Results.NoContent();
            //});

            // Add the endpoint for IndexController
            app.MapGet("/api/index", async (HttpContext context, [FromServices] IInvertedIndexService invertedIndexService) =>
            {
                var indexService = context.RequestServices.GetRequiredService<InvertedIndexService>;
                await invertedIndexService.InitializeUser();
                return Results.Ok();
            });

            app.MapPost("/api/search", async (HttpContext context, [FromServices] ISearchService searchService, [FromBody] SearchDetailsDTO searchDetails) =>
            {
                if (searchDetails == null || string.IsNullOrEmpty(searchDetails.searchwords))
                {
                    return Results.BadRequest("Search details cannot be empty");
                }

                IEnumerable<string> searchQueries = await searchService.ProcessSearchQuery(searchDetails.searchwords);
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
                    var searchResult = await searchService.BoolSearch(query);
                    return Results.Ok(searchResult);
                }
                return Results.BadRequest("No valid search terms found"); 
            });

/*            app.MapGet("/api/GetAllSearch", async ([FromServices] ISearchService searchService) =>
            {

                IEnumerable<DocumentInformation> document = await searchService.GetALlDocumentsWithIndex();
                foreach (var doc in document)
                {
                    foreach (var index in doc.TermDocuments)
                    {
                        index.DocumentInformation = null;
                    }
                }
                return Results.Ok(document);
            });*/

            app.MapGet("/api/messages", (SampleData data, string q = "") => data.Data);

        }
    }
}
