using P7_PSEngine.Data;
using P7_PSEngine.Model;

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

            app.MapPost("/api/products", async (Todo todos, TodoDb db) =>
            {
                db.Todos.Add(todos);
                await db.SaveChangesAsync();
                return Results.Created($"/api/products/{todos.Id}", todos);
            });

            // app.MapPut("/api/products/{id}", async (int id, Todo todo, TodoDb db) =>
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

            app.MapDelete("/api/products/{id}", async (int id, TodoDb db) =>
            {
                var product = await db.Todos.FindAsync(id);
                if (product == null)
                {
                    return Results.NotFound();
                }

                db.Todos.Remove(product);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            //string static_path = "/app/wwwroot";

            //Lets you send files from wwwroot folder
            app.UseStaticFiles();

            //examples on how you can send data or pages
            //Send html page from wwwroot folder
            //app.MapGet("/", () => Results.Content(File.ReadAllText($"{static_path}/index.html"), "text/html"));

            //app.MapGet("/linkuser", () => Results.Content(File.ReadAllText($"{static_path}/html/auth.html"), "text/html"));

            //Send a JSON object
            app.MapGet("/api/test", () => new { hmm = "wow", bab = 12345 });

            //Send a string
            app.MapGet("/api/test2", () => "test2");

            //Use variables from URL
            app.MapGet("/api/repeat/{message}", (string message) => $"{message}");

            // Add the endpoint for IndexController
            app.MapGet("/api/index", async (HttpContext context) =>
            {
                var indexController = context.RequestServices.GetRequiredService<IndexController>();
                var result = indexController.GetIndexData();
                return Results.Json(result);
            });

            app.MapGet("/api/search", async (HttpContext context) =>
            {
                var searchController = context.RequestServices.GetRequiredService<SearchController>();
                var searchTerm = context.Request.Query["q"].ToString();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    return Results.BadRequest("Invalid search term");
                }
                var result = searchController.Search(searchTerm);
                return Results.Json(result);
            });

        }
    }
}
