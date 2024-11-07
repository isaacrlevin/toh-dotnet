using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using tohdotnet.domain;
using tohdotnet.domain.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddSnapshotCollector();
builder.Services.AddTransient<IHeroService, HeroService>();

builder.Services.AddDbContext<tohdotnetContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("tohdotnetContext")));

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/Heroes", async (IHeroService service, string? name) =>
{
    if (!string.IsNullOrEmpty(name))
    {
        return await service.SearchHeros(name);
    }
    else
    {
        return await service.GetHeros();
    }
})
    .WithName("GetHeroes");

app.MapGet("/Heroes/{id}", async (IHeroService service, int id) =>
{
    return await service.GetHero(id);
});

app.MapPut("/Heroes/{id}", async (IHeroService service, int id, Hero hero) =>
{
    try
    {
        await service.UpdateHero(id, hero);
    }
    catch (DbUpdateConcurrencyException)
    {
        var tempHero = await service.GetHero(id);
        if (tempHero != null)
        {
            throw;
        }
    }
});

app.MapPost("/Heroes", async (IHeroService service, Hero hero) =>
{
    return await service.CreateHero(hero);
});

app.MapDelete("/Heroes/{id}", async (IHeroService service, int id) =>
{
    var hero = await service.GetHero(id);
    if (hero != null)
    {
        await service.DeleteHero(hero);
    }
});

app.MapFallbackToFile("/index.html");

app.Run();
