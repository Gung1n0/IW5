using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Common.Models.Place;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IW5_project.Api.App.EndToEndTests;

/// <summary>
/// xUnit creates a new instance of the test class for every test, so every test gets
/// its own API instance and its own empty database.
/// </summary>
public abstract class ApiTestBase : IAsyncDisposable
{
    protected IW5ApiApplicationFactory Factory { get; } = new();
    protected HttpClient Client { get; }

    protected ApiTestBase()
    {
        Client = Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),   // https => UseHttpsRedirection() never redirects
            AllowAutoRedirect = false
        });
    }

    // ---------- seeding (direct DB access; there are no Item endpoints yet) ----------
    
    protected Guid SeedUser(string name = "John Doe", string role = "user", string? photoUrl = null)
    {
        var id = Guid.NewGuid();
        Factory.Seed(db => db.Users.Add(new UserEntity
        {
            Id = id,
            Name = name,
            Role = role,
            PhotoUrl = photoUrl
        }));
        return id;
    }

    protected Guid SeedItem(string name = "Milk")
    {
        var id = Guid.NewGuid();
        Factory.Seed(db => db.Items.Add(new ItemEntity { Id = id, Name = name, Quantity = 1 }));
        return id;
    }

    protected Guid SeedPlace(string name, string? description = null, int itemCount = 0)
    {
        var id = Guid.NewGuid();
        Factory.Seed(db => db.Places.Add(new PlaceEntity
        {
            Id = id,
            Name = name,
            Description = description,
            Items = Enumerable.Range(1, itemCount)
                .Select(i => new ItemEntity { Id = Guid.NewGuid(), Name = $"{name} item {i}" })
                .ToList()
        }));
        return id;
    }

    protected Guid SeedShoppingList(string name, string store, string? description = null)
    {
        var id = Guid.NewGuid();
        Factory.Seed(db => db.ShoppingLists.Add(new ShoppingListEntity
        {
            Id = id,
            Name = name,
            Store = store,
            Description = description
        }));
        return id;
    }

    // ---------- assertions ----------

    /// <summary>422 + ProblemDetails whose "errors" contain the given field (case-insensitive).</summary>
    protected static async Task AssertValidationErrorAsync(HttpResponseMessage response, string expectedField)
    {
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        var fields = body.GetProperty("errors").EnumerateObject().Select(p => p.Name).ToList();

        Assert.Contains(fields, f => string.Equals(f, expectedField, StringComparison.OrdinalIgnoreCase));
    }

    public async ValueTask DisposeAsync()
    {
        Client.Dispose();
        await Factory.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
