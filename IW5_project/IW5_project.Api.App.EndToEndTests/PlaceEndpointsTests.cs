using System.Net;
using System.Net.Http.Json;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models;
using IW5_project.Common.Models.Place;

namespace IW5_project.Api.App.EndToEndTests;

public class PlaceEndpointsTests : ApiTestBase
{
    private async Task<PagedResult<PlaceListModel>> GetPlacesAsync(string query = "")
    {
        var response = await Client.GetAsync($"/api/place?{query}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var page = await response.Content.ReadFromJsonAsync<PagedResult<PlaceListModel>>();
        Assert.NotNull(page);
        return page;
    }

    private async Task<PlaceDetailModel> GetPlaceAsync(Guid id)
    {
        var response = await Client.GetAsync($"/api/place/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var place = await response.Content.ReadFromJsonAsync<PlaceDetailModel>();
        Assert.NotNull(place);
        return place;
    }

    private async Task<Guid> CreatePlaceAsync(string name, string? description = null)
    {
        var response = await Client.PostAsJsonAsync("/api/place",
            new PlaceDetailModel { Name = name, Description = description });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    // GET /api/place 

    [Fact]
    public async Task GetPlaces_EmptyDatabase_ReturnsEmptyPage()
    {
        var result = await GetPlacesAsync();

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public async Task GetPlaces_Paging_SplitsResultsIntoPages()
    {
        for (var i = 1; i <= 12; i++)
        {
            SeedPlace($"Place {i:00}");
        }

        var first = await GetPlacesAsync();
        var second = await GetPlacesAsync("page=2");

        Assert.Equal(10, first.Items.Count);
        Assert.Equal(2, second.Items.Count);
        Assert.Equal(12, first.TotalCount);
        Assert.Equal(2, first.TotalPages);
    }

    [Theory]
    [InlineData("Fri", "Fridge")]
    [InlineData("dry", "Pantry")]
    public async Task GetPlaces_Search_MatchesNameAndDescription(string search, string expectedName)
    {
        SeedPlace("Fridge", "cold storage");
        SeedPlace("Pantry", "dry goods");

        var result = await GetPlacesAsync($"search={search}");

        Assert.Equal(expectedName, Assert.Single(result.Items).Name);
    }

    [Fact]
    public async Task GetPlaces_SortByNameDescending_ReturnsReversedOrder()
    {
        SeedPlace("B place");
        SeedPlace("C place");
        SeedPlace("A place");

        var result = await GetPlacesAsync("sortBy=Name&sortDirection=Descending");

        Assert.Equal(new[] { "C place", "B place", "A place" }, result.Items.Select(p => p.Name));
    }

    [Fact]
    public async Task GetPlaces_SortByItemCount_OrdersByNumberOfItems()
    {
        SeedPlace("Three", itemCount: 3);
        SeedPlace("One", itemCount: 1);
        SeedPlace("Two", itemCount: 2);

        var result = await GetPlacesAsync("sortBy=ItemCount");

        Assert.Equal(new[] { "One", "Two", "Three" }, result.Items.Select(p => p.Name));
    }

    [Theory]
    [InlineData("page=0", "page")]
    [InlineData("pageSize=101", "pageSize")]
    [InlineData("sortBy=99", "SortBy")]
    public async Task GetPlaces_InvalidQuery_Returns422(string query, string expectedField)
    {
        var response = await Client.GetAsync($"/api/place?{query}");

        await AssertValidationErrorAsync(response, expectedField);
    }

    // GET /api/place/{id}

    [Fact]
    public async Task GetPlaceById_ExistingPlace_ReturnsDetail()
    {
        var id = SeedPlace("Pantry", "dry goods", itemCount: 2);

        var place = await GetPlaceAsync(id);

        Assert.Equal("Pantry", place.Name);
        Assert.Equal("dry goods", place.Description);
        Assert.Equal(2, place.ItemCount);
    }

    [Fact]
    public async Task GetPlaceById_UnknownId_Returns404()
    {
        var response = await Client.GetAsync($"/api/place/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // POST /api/place

    [Fact]
    public async Task CreatePlace_ValidModel_Returns201AndPlaceCanBeRead()
    {
        var response = await Client.PostAsJsonAsync("/api/place",
            new PlaceDetailModel { Name = "Fridge", Description = "Kitchen" });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var id = await response.Content.ReadFromJsonAsync<Guid>();
        Assert.NotEqual(Guid.Empty, id);

        var created = await GetPlaceAsync(id);
        Assert.Equal("Fridge", created.Name);
        Assert.Equal("Kitchen", created.Description);
    }

    [Fact]
    public async Task CreatePlace_MissingName_Returns422()
    {
        var response = await Client.PostAsJsonAsync("/api/place", new PlaceDetailModel { Name = "" });

        await AssertValidationErrorAsync(response, "Name");
    }

    [Fact]
    public async Task CreatePlace_NameTooLong_Returns422()
    {
        var model = new PlaceDetailModel { Name = new string('a', Constants.MAX_NAME_LENGTH + 1) };

        var response = await Client.PostAsJsonAsync("/api/place", model);

        await AssertValidationErrorAsync(response, "Name");
    }

    // PUT /api/place

    [Fact]
    public async Task UpdatePlace_ExistingPlace_Returns200AndChangesAreStored()
    {
        var id = await CreatePlaceAsync("Fridge", "old");

        var response = await Client.PutAsJsonAsync("/api/place",
            new PlaceDetailModel { Id = id, Name = "Freezer", Description = "new" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await GetPlaceAsync(id);
        Assert.Equal("Freezer", updated.Name);
        Assert.Equal("new", updated.Description);
    }

    [Fact]
    public async Task UpdatePlace_UnknownId_Returns404()
    {
        var response = await Client.PutAsJsonAsync("/api/place",
            new PlaceDetailModel { Id = Guid.NewGuid(), Name = "Ghost" });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // DELETE /api/place/{id}

    [Fact]
    public async Task DeletePlace_EmptyPlace_Returns200AndPlaceIsGone()
    {
        var id = await CreatePlaceAsync("Fridge");

        var response = await Client.DeleteAsync($"/api/place/{id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync($"/api/place/{id}")).StatusCode);
    }

    [Fact]
    public async Task DeletePlace_UnknownId_Returns404()
    {
        var response = await Client.DeleteAsync($"/api/place/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeletePlace_PlaceStillContainsItems_Returns409()
    {
        var id = SeedPlace("Pantry", itemCount: 2);

        var response = await Client.DeleteAsync($"/api/place/{id}");

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync($"/api/place/{id}")).StatusCode);
    }
}
