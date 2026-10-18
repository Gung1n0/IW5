using System.Net;
using System.Net.Http.Json;
using IW5_project.Api.Common.Models.Common;
using IW5_project.Common.Models.ShoppingList;

namespace IW5_project.Api.App.EndToEndTests;

public class ShoppingListEndpointsTests : ApiTestBase
{
    // helper methods
    private async Task<Guid> CreateListAsync(string name = "Weekly shop", string store = "Tesco")
    {
        var response = await Client.PostAsJsonAsync("/api/shopping-list",
            new ShoppingListDetailModel { Name = name, Store = store });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    private async Task AddItemAsync(Guid listId, Guid itemId, int quantity = 1)
    {
        var response = await Client.PostAsJsonAsync($"/api/shopping-list/{listId}/items",
            new AddShoppingListItemModel { ItemId = itemId, Quantity = quantity });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    private async Task<ShoppingListDetailModel> GetListAsync(Guid id)
    {
        var response = await Client.GetAsync($"/api/shopping-list/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var list = await response.Content.ReadFromJsonAsync<ShoppingListDetailModel>();
        Assert.NotNull(list);
        return list;
    }

    private async Task<PagedResult<ShoppingListListModel>> GetListsAsync(string query = "")
    {
        var response = await Client.GetAsync($"/api/shopping-list?{query}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var page = await response.Content.ReadFromJsonAsync<PagedResult<ShoppingListListModel>>();
        Assert.NotNull(page);
        return page;
    }

    // GET /api/shopping-list 

    [Fact]
    public async Task GetLists_EmptyDatabase_ReturnsEmptyPage()
    {
        var result = await GetListsAsync();

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public async Task GetLists_Paging_SplitsResultsIntoPages()
    {
        for (var i = 1; i <= 12; i++)
        {
            SeedShoppingList($"List {i:00}", "Tesco");
        }

        var first = await GetListsAsync();
        var second = await GetListsAsync("page=2");

        Assert.Equal(10, first.Items.Count);
        Assert.Equal(2, second.Items.Count);
        Assert.Equal(12, first.TotalCount);
    }

    [Theory]
    [InlineData("Weekly", "Weekly shop")]   // name
    [InlineData("Albert", "Party")]         // store
    [InlineData("birthday", "Party")]       // description
    public async Task GetLists_Search_MatchesNameStoreAndDescription(string search, string expectedName)
    {
        SeedShoppingList("Weekly shop", "Tesco", "basics");
        SeedShoppingList("Party", "Albert", "birthday snacks");

        var result = await GetListsAsync($"search={search}");

        Assert.Equal(expectedName, Assert.Single(result.Items).Name);
    }

    [Fact]
    public async Task GetLists_FilterByStore_ReturnsOnlyListsFromThatStore()
    {
        SeedShoppingList("A", "Lidl");
        SeedShoppingList("B", "Tesco");
        SeedShoppingList("C", "Lidl");

        var result = await GetListsAsync("store=Lidl");

        Assert.Equal(2, result.TotalCount);
        Assert.All(result.Items, l => Assert.Equal("Lidl", l.Store));
    }

    [Fact]
    public async Task GetLists_SortByItemCount_OrdersByNumberOfItems()
    {
        var two = await CreateListAsync("Two");
        var empty = await CreateListAsync("Empty");
        var one = await CreateListAsync("One");
        await AddItemAsync(two, SeedItem("Milk"));
        await AddItemAsync(two, SeedItem("Bread"));
        await AddItemAsync(one, SeedItem("Eggs"));

        var result = await GetListsAsync("sortBy=ItemCount");

        Assert.Equal(new[] { empty, one, two }, result.Items.Select(l => l.Id));
        Assert.Equal(new[] { 0, 1, 2 }, result.Items.Select(l => l.ItemCount));
    }

    [Theory]
    [InlineData("page=0", "page")]
    [InlineData("pageSize=101", "pageSize")]
    [InlineData("sortBy=99", "SortBy")]
    public async Task GetLists_InvalidQuery_Returns422(string query, string expectedField)
    {
        var response = await Client.GetAsync($"/api/shopping-list?{query}");

        await AssertValidationErrorAsync(response, expectedField);
    }

    // GET /api/shopping-list/{id} 

    [Fact]
    public async Task GetListById_ExistingList_ReturnsDetailWithItems()
    {
        var listId = await CreateListAsync("Weekly shop", "Tesco");
        var milk = SeedItem("Milk");
        await AddItemAsync(listId, milk, quantity: 2);

        var list = await GetListAsync(listId);

        Assert.Equal("Weekly shop", list.Name);
        Assert.Equal("Tesco", list.Store);
        var line = Assert.Single(list.Items);
        Assert.Equal(milk, line.ItemId);
        Assert.Equal(2, line.Quantity);
        Assert.False(line.IsBought);
    }

    [Fact]
    public async Task GetListById_UnknownId_Returns404()
    {
        var response = await Client.GetAsync($"/api/shopping-list/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // POST /api/shopping-list

    [Fact]
    public async Task CreateList_ValidModel_Returns201AndListCanBeRead()
    {
        var response = await Client.PostAsJsonAsync("/api/shopping-list",
            new ShoppingListDetailModel { Name = "Party", Store = "Albert", Description = "Saturday" });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var id = await response.Content.ReadFromJsonAsync<Guid>();

        var created = await GetListAsync(id);
        Assert.Equal("Party", created.Name);
        Assert.Equal("Albert", created.Store);
        Assert.Equal("Saturday", created.Description);
        Assert.Empty(created.Items);
    }

    [Theory]
    [InlineData("", "Tesco", "Name")]
    [InlineData("Weekly shop", "", "Store")]
    public async Task CreateList_MissingRequiredField_Returns422(string name, string store, string expectedField)
    {
        var response = await Client.PostAsJsonAsync("/api/shopping-list",
            new ShoppingListDetailModel { Name = name, Store = store });

        await AssertValidationErrorAsync(response, expectedField);
    }

    // PUT /api/shopping-list

    [Fact]
    public async Task UpdateList_ExistingList_Returns200AndChangesAreStored()
    {
        var id = await CreateListAsync("Weekly shop", "Tesco");

        var response = await Client.PutAsJsonAsync("/api/shopping-list",
            new ShoppingListDetailModel { Id = id, Name = "Party", Store = "Albert" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await GetListAsync(id);
        Assert.Equal("Party", updated.Name);
        Assert.Equal("Albert", updated.Store);
    }

    [Fact]
    public async Task UpdateList_UnknownId_Returns404()
    {
        var response = await Client.PutAsJsonAsync("/api/shopping-list",
            new ShoppingListDetailModel { Id = Guid.NewGuid(), Name = "Ghost", Store = "Tesco" });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // DELETE /api/shopping-list/{id}

    [Fact]
    public async Task DeleteList_ExistingList_Returns200AndRemovesItsLinesButNotTheItems()
    {
        var listId = await CreateListAsync();
        var itemId = SeedItem("Milk");
        await AddItemAsync(listId, itemId);

        var response = await Client.DeleteAsync($"/api/shopping-list/{listId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync($"/api/shopping-list/{listId}")).StatusCode);
        Assert.Equal(0, Factory.Query(db => db.ShoppingListItems.Count()));
        Assert.True(Factory.Query(db => db.Items.Any(i => i.Id == itemId)));
    }

    [Fact]
    public async Task DeleteList_UnknownId_Returns404()
    {
        var response = await Client.DeleteAsync($"/api/shopping-list/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // POST /api/shopping-list/{id}/items

    [Fact]
    public async Task AddItem_ValidRequest_Returns201AndItemAppearsOnList()
    {
        var listId = await CreateListAsync();
        var itemId = SeedItem("Milk");

        var response = await Client.PostAsJsonAsync($"/api/shopping-list/{listId}/items",
            new AddShoppingListItemModel { ItemId = itemId, Quantity = 3 });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var line = Assert.Single((await GetListAsync(listId)).Items);
        Assert.Equal(itemId, line.ItemId);
        Assert.Equal(3, line.Quantity);
    }

    [Fact]
    public async Task AddItem_UnknownList_Returns404()
    {
        var response = await Client.PostAsJsonAsync($"/api/shopping-list/{Guid.NewGuid()}/items",
            new AddShoppingListItemModel { ItemId = SeedItem("Milk"), Quantity = 1 });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddItem_UnknownItem_Returns400()
    {
        var listId = await CreateListAsync();

        var response = await Client.PostAsJsonAsync($"/api/shopping-list/{listId}/items",
            new AddShoppingListItemModel { ItemId = Guid.NewGuid(), Quantity = 1 });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddItem_ItemAlreadyOnList_Returns409()
    {
        var listId = await CreateListAsync();
        var itemId = SeedItem("Milk");
        await AddItemAsync(listId, itemId);

        var response = await Client.PostAsJsonAsync($"/api/shopping-list/{listId}/items",
            new AddShoppingListItemModel { ItemId = itemId, Quantity = 1 });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task AddItem_QuantityZero_Returns422()
    {
        var listId = await CreateListAsync();

        var response = await Client.PostAsJsonAsync($"/api/shopping-list/{listId}/items",
            new AddShoppingListItemModel { ItemId = SeedItem("Milk"), Quantity = 0 });

        await AssertValidationErrorAsync(response, "Quantity");
    }

    // PUT /api/shopping-list/{id}/items/{itemId}

    [Fact]
    public async Task UpdateItem_ExistingLine_Returns200AndChangesAreStored()
    {
        var listId = await CreateListAsync();
        var itemId = SeedItem("Milk");
        await AddItemAsync(listId, itemId, quantity: 1);

        var response = await Client.PutAsJsonAsync($"/api/shopping-list/{listId}/items/{itemId}",
            new UpdateShoppingListItemModel { Quantity = 6, IsBought = true });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var line = Assert.Single((await GetListAsync(listId)).Items);
        Assert.Equal(6, line.Quantity);
        Assert.True(line.IsBought);
    }

    [Fact]
    public async Task UpdateItem_ItemIsNotOnTheList_Returns404()
    {
        var listId = await CreateListAsync();
        var itemId = SeedItem("Milk");

        var response = await Client.PutAsJsonAsync($"/api/shopping-list/{listId}/items/{itemId}",
            new UpdateShoppingListItemModel { Quantity = 2, IsBought = false });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // DELETE /api/shopping-list/{id}/items/{itemId}

    [Fact]
    public async Task RemoveItem_ExistingLine_Returns200AndLineIsGone()
    {
        var listId = await CreateListAsync();
        var itemId = SeedItem("Milk");
        await AddItemAsync(listId, itemId);

        var response = await Client.DeleteAsync($"/api/shopping-list/{listId}/items/{itemId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Empty((await GetListAsync(listId)).Items);
        Assert.True(Factory.Query(db => db.Items.Any(i => i.Id == itemId)));
    }

    [Fact]
    public async Task RemoveItem_ItemIsNotOnTheList_Returns404()
    {
        var listId = await CreateListAsync();

        var response = await Client.DeleteAsync($"/api/shopping-list/{listId}/items/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
