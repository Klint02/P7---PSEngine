using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;

namespace P7_PSEngine.API
{
    public static class ProductEndpointsExt
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            app.MapGet("/api/products", async (TodoDb db) =>
            {
                var products = await db.Todos.ToListAsync();
                return products;
            });

            app.MapGet("/todoitems", async (TodoDb db) =>
                await db.Todos.ToListAsync());

            app.MapGet("/api/products/{id}", async (HttpContext context, TodoDb db) =>
            {
                if (!int.TryParse(context.Request.RouteValues["id"]?.ToString(), out var id))
                {
                    return Results.BadRequest("Invalid product ID");
                }

                var product = await db.Todos.FindAsync(id);
                if (product == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(product);
            });

            app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
                await db.Todos.FindAsync(id)
                    is Todo todo
                        ? Results.Ok(todo)
                        : Results.NotFound());

            //app.MapPost("/api/products", async (Todos todos) =>
            //{
            //    db.Products.Add(product);
            //    await db.SaveChangesAsync();
            //    return Results.Created($"/api/products/{product.Id}", product);
            //});

            //app.MapPut("/api/products/{id}", async (int id, Product product) =>
            //{
            //    if (id != product.Id)
            //    {
            //        return Results.BadRequest();
            //    }

            //    db.Entry(product).State = EntityState.Modified;

            //    try
            //    {
            //        await db.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (await db.Products.FindAsync(id) == null)
            //        {
            //            return Results.NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }

            //    return Results.NoContent();
            //});

            //app.MapDelete("/api/products/{id}", async (int id) =>
            //{
            //    var product = await db.Products.FindAsync(id);
            //    if (product == null)
            //    {
            //        return Results.NotFound();
            //    }

            //    db.Products.Remove(product);
            //    await db.SaveChangesAsync();

            //    return Results.NoContent();
            //});
        }
    }
}
