using Context;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Docker.API.Endpoints
{
    public static class PeopleEndpoints
    {
        public static void MapPeopleEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/people", async (AppDbContext context) =>
            {
                return await context.People.ToListAsync();
            });

            app.MapGet("/api/people/{id}", async (int id, AppDbContext context) =>
            {
                var person = await context.People.FindAsync(id);
                return person != null ? Results.Ok(person) : Results.NotFound();
            });

            app.MapPut("/api/people/{id}", async (int id, Person person, AppDbContext context) =>
            {
                if (id != person.Id)
                {
                    return Results.BadRequest();
                }

                context.Entry(person).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!context.People.Any(e => e.Id == id))
                    {
                        return Results.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Results.NoContent();
            });

            app.MapPost("/api/people", async (Person person, AppDbContext context) =>
            {
                context.People.Add(person);
                await context.SaveChangesAsync();

                return Results.Created($"/api/people/{person.Id}", person);
            });

            app.MapDelete("/api/people/{id}", async (int id, AppDbContext context) =>
            {
                var person = await context.People.FindAsync(id);
                if (person == null)
                {
                    return Results.NotFound();
                }

                context.People.Remove(person);
                await context.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}
