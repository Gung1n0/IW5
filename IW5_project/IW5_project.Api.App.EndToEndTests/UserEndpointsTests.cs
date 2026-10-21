using System.Net;
using System.Net.Http.Json;
using IW5_project.Api.DAL.EF;
using IW5_project.Common.Models;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.Place;
using IW5_project.Common.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IW5_project.Api.App.EndToEndTests;

public class UserEndpointsTests : ApiTestBase
{
    // ==========================================
    // Helper Methods
    // ==========================================

    private async Task<Guid> CreateUserViaApiAsync(
        string name = "Test User",
        string role = UserRoles.User,
        string? photoUrl = null,
        string? callerRole = UserRoles.Admin,
        string? callerId = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(Guid.NewGuid(), name, photoUrl, role))
        };

        if (!string.IsNullOrWhiteSpace(callerRole))
        {
            request.Headers.Add("X-User-Role", callerRole);
        }

        if (!string.IsNullOrWhiteSpace(callerId))
        {
            request.Headers.Add("X-User-Id", callerId);
        }

        var response = await Client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    private async Task<UserDetailModel> GetUserAsync(Guid id)
    {
        var response = await Client.GetAsync($"/api/user/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var user = await response.Content.ReadFromJsonAsync<UserDetailModel>();
        Assert.NotNull(user);
        return user;
    }

    private async Task<PagedResult<UserListModel>> GetUsersAsync(string query = "")
    {
        var response = await Client.GetAsync($"/api/user?{query}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var page = await response.Content.ReadFromJsonAsync<PagedResult<UserListModel>>();
        Assert.NotNull(page);
        return page;
    }

    // ==========================================
    // 1. List, Search, and Pagination (GET /api/user)
    // ==========================================

    [Fact]
    public async Task TC01_GetUsers_Paging_SplitsResultsAndCalculatesMetadataCorrectly()
    {
        // Arrange: 25 users in the database
        for (var i = 1; i <= 25; i++)
        {
            SeedUser($"User {i:00}");
        }

        // Act & Assert - pageSize=10
        var page1 = await GetUsersAsync("page=1&pageSize=10");
        var page2 = await GetUsersAsync("page=2&pageSize=10");
        var page3 = await GetUsersAsync("page=3&pageSize=10");

        Assert.Equal(10, page1.Items.Count);
        Assert.Equal(10, page2.Items.Count);
        Assert.Equal(5, page3.Items.Count);
        Assert.Equal(25, page1.TotalCount);
        Assert.Equal(3, page1.TotalPages);

        // No overlaps between pages
        var idsPage1 = page1.Items.Select(u => u.Id).ToHashSet();
        Assert.DoesNotContain(page2.Items, u => idsPage1.Contains(u.Id));

        // Act & Assert - pageSize=5
        var smallPage = await GetUsersAsync("page=1&pageSize=5");
        Assert.Equal(5, smallPage.Items.Count);
        Assert.Equal(25, smallPage.TotalCount);
        Assert.Equal(5, smallPage.TotalPages);

        // Act & Assert - pageSize=50
        var largePage = await GetUsersAsync("page=1&pageSize=50");
        Assert.Equal(25, largePage.Items.Count);
        Assert.Equal(25, largePage.TotalCount);
        Assert.Equal(1, largePage.TotalPages);
    }

    [Theory]
    [InlineData("page=0", "page")]
    [InlineData("pageSize=0", "pageSize")]
    [InlineData("pageSize=101", "pageSize")]
    public async Task TC02_GetUsers_InvalidPagingParameters_Returns422(string query, string expectedField)
    {
        // Act
        var response = await Client.GetAsync($"/api/user?{query}");

        // Assert
        await AssertValidationErrorAsync(response, expectedField);
    }

    [Fact]
    public async Task TC03_GetUsers_Search_MatchesSubstringAndTrimsWhitespace()
    {
        // Arrange
        SeedUser("Jan Novák");
        SeedUser("Bohuslav Novák");
        SeedUser("Petr Svoboda");

        // Act: Search with white characters on the edges
        var result = await GetUsersAsync("search=%20%20Novák%20%20");

        // Assert
        Assert.Equal(2, result.TotalCount);
        Assert.All(result.Items, u => Assert.Contains("Novák", u.Name));
    }

    [Fact]
    public async Task TC04_GetUsers_Search_CaseInsensitivityAndDiacritics()
    {
        // Arrange
        SeedUser("Štěpán Řehoř");

        // Act: Search using exact diacritics and lowercase letters
        var exactResult = await GetUsersAsync("search=Štěpán");
        var lowerResult = await Client.GetAsync("/api/user?search=štěpán");

        // Assert
        Assert.Single(exactResult.Items);
        Assert.Equal(HttpStatusCode.OK, lowerResult.StatusCode);
    }

    [Fact]
    public async Task TC05_GetUsers_SortDirection_OrdersAscendingAndDescending()
    {
        // Arrange
        SeedUser("Adam");
        SeedUser("Cyril");
        SeedUser("Bedřich");

        // Act
        var asc = await GetUsersAsync("sortDirection=Ascending");
        var desc = await GetUsersAsync("sortDirection=Descending");

        // Assert
        Assert.Equal(new[] { "Adam", "Bedřich", "Cyril" }, asc.Items.Select(u => u.Name));
        Assert.Equal(new[] { "Cyril", "Bedřich", "Adam" }, desc.Items.Select(u => u.Name));
    }

    [Fact]
    public async Task TC06_GetUsers_CombinedQuery_SearchPagingAndSortingAppliedTogether()
    {
        // Arrange
        for (var i = 1; i <= 25; i++)
        {
            SeedUser($"Tester {i:00}");
        }
        for (var i = 1; i <= 5; i++)
        {
            SeedUser($"Other {i:00}");
        }

        // Act: Page 2, displaying 5 items from the Tester search list, sorted in descending order
        var result = await GetUsersAsync("search=Tester&sortDirection=Descending&page=2&pageSize=5");

        // Assert
        Assert.Equal(25, result.TotalCount);
        Assert.Equal(5, result.Items.Count);
        // In descending order: Page 1 has Tester 25–21, Page 2 has Tester 20–16
        Assert.Equal("Tester 20", result.Items.First().Name);
        Assert.Equal("Tester 16", result.Items.Last().Name);
    }

    // ==========================================
    // 2. Detail uživatele (GET /api/user/{id})
    // ==========================================

    [Fact]
    public async Task TC07_GetUserById_ExistingUser_ReturnsAllAttributes()
    {
        // Arrange
        var userId = SeedUser("Jan Novák", UserRoles.Admin, "https://example.com/avatar.jpg");

        // Act
        var user = await GetUserAsync(userId);

        // Assert
        Assert.Equal(userId, user.Id);
        Assert.Equal("Jan Novák", user.Name);
        Assert.Equal(UserRoles.Admin, user.Role);
        Assert.Equal("https://example.com/avatar.jpg", user.PhotoUrl);
    }

    [Fact]
    public async Task TC08_GetUserById_UnknownId_Returns404NotFound()
    {
        // Act
        var response = await Client.GetAsync($"/api/user/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task TC09_GetUserById_InvalidGuidFormat_Returns404NotFound()
    {
        // Act
        var response = await Client.GetAsync("/api/user/not-a-valid-guid");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ==========================================
    // 3. Creating a User (POST /api/user)
    // ==========================================

    [Fact]
    public async Task TC10_CreateUser_AdminCreatesUserAndAdmin_Returns201WithLocation()
    {
        // Arrange: Admin creating regular user
        using var requestUser = new HttpRequestMessage(HttpMethod.Post, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(Guid.NewGuid(), "Regular Karel", null, UserRoles.User))
        };
        requestUser.Headers.Add("X-User-Role", UserRoles.Admin);

        // Act
        var responseUser = await Client.SendAsync(requestUser);

        // Assert
        Assert.Equal(HttpStatusCode.Created, responseUser.StatusCode);
        Assert.NotNull(responseUser.Headers.Location);
        var createdId = await responseUser.Content.ReadFromJsonAsync<Guid>();

        var userDetail = await GetUserAsync(createdId);
        Assert.Equal("Regular Karel", userDetail.Name);
        Assert.Equal(UserRoles.User, userDetail.Role);

        // Arrange: Admin creating another admin
        using var requestAdmin = new HttpRequestMessage(HttpMethod.Post, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(Guid.NewGuid(), "Admin Alice", null, UserRoles.Admin))
        };
        requestAdmin.Headers.Add("X-User-Role", UserRoles.Admin);

        // Act
        var responseAdmin = await Client.SendAsync(requestAdmin);

        // Assert
        Assert.Equal(HttpStatusCode.Created, responseAdmin.StatusCode);
    }

    [Fact]
    public async Task TC11_CreateUser_RegularUserAttemptsToCreateAdmin_Returns403Forbidden()
    {
        // Arrange: Regular user attempting privilege escalation
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(Guid.NewGuid(), "Malicious Admin", null, UserRoles.Admin))
        };
        request.Headers.Add("X-User-Role", UserRoles.User);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public async Task TC12_CreateUser_EmptyOrWhitespaceName_Returns422(string invalidName)
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(Guid.NewGuid(), invalidName, null, UserRoles.User))
        };
        request.Headers.Add("X-User-Role", UserRoles.Admin);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        await AssertValidationErrorAsync(response, "Name");
    }

    [Fact]
    public async Task TC13_CreateUser_NameLengthBoundary_100CharsAllowed_101CharsReturns422()
    {
        // Arrange 1: Exact 100 characters
        var validName = new string('A', 100);
        using var requestValid = new HttpRequestMessage(HttpMethod.Post, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(Guid.NewGuid(), validName, null, UserRoles.User))
        };
        requestValid.Headers.Add("X-User-Role", UserRoles.Admin);

        // Act 1
        var responseValid = await Client.SendAsync(requestValid);
        Assert.Equal(HttpStatusCode.Created, responseValid.StatusCode);

        // Arrange 2: 101 characters (overflow)
        var invalidName = new string('A', 101);
        using var requestInvalid = new HttpRequestMessage(HttpMethod.Post, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(Guid.NewGuid(), invalidName, null, UserRoles.User))
        };
        requestInvalid.Headers.Add("X-User-Role", UserRoles.Admin);

        // Act 2
        var responseInvalid = await Client.SendAsync(requestInvalid);

        // Assert 2
        await AssertValidationErrorAsync(responseInvalid, "Name");
    }

    [Theory]
    [InlineData("superadmin")]
    [InlineData("guest")]
    [InlineData("")]
    public async Task TC14_CreateUser_InvalidRole_Returns422(string invalidRole)
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(Guid.NewGuid(), "Valid Name", null, invalidRole))
        };
        request.Headers.Add("X-User-Role", UserRoles.Admin);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        await AssertValidationErrorAsync(response, "Role");
    }

    [Fact]
    public async Task TC15_CreateUser_PhotoUrlValidation_NullAllowed_Over2048CharsReturns422()
    {
        // Arrange 1: PhotoUrl = null is allowed
        using var requestValid = new HttpRequestMessage(HttpMethod.Post, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(Guid.NewGuid(), "Valid User", null, UserRoles.User))
        };
        requestValid.Headers.Add("X-User-Role", UserRoles.Admin);

        var responseValid = await Client.SendAsync(requestValid);
        Assert.Equal(HttpStatusCode.Created, responseValid.StatusCode);

        // Arrange 2: PhotoUrl > 2048 chars
        var invalidPhotoUrl = "https://example.com/" + new string('x', 2050);
        using var requestInvalid = new HttpRequestMessage(HttpMethod.Post, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(Guid.NewGuid(), "Valid User 2", invalidPhotoUrl, UserRoles.User))
        };
        requestInvalid.Headers.Add("X-User-Role", UserRoles.Admin);

        var responseInvalid = await Client.SendAsync(requestInvalid);
        await AssertValidationErrorAsync(responseInvalid, "PhotoUrl");
    }

    // ==========================================
    // 4. Editing an existing User (PUT /api/user)
    // ==========================================

    [Fact]
    public async Task TC16_UpdateUser_UserUpdatesOwnProfileNameAndPhoto_Returns200()
    {
        // Arrange
        var userId = SeedUser("Initial Name", UserRoles.User, null);

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(userId, "Updated Name", "https://example.com/avatar.png", UserRoles.User))
        };
        request.Headers.Add("X-User-Id", userId.ToString());
        request.Headers.Add("X-User-Role", UserRoles.User);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updated = await GetUserAsync(userId);
        Assert.Equal("Updated Name", updated.Name);
        Assert.Equal("https://example.com/avatar.png", updated.PhotoUrl);
        Assert.Equal(UserRoles.User, updated.Role);
    }

    [Fact]
    public async Task TC17_UpdateUser_UserAttemptsToEscalateRoleToAdmin_Returns403Forbidden()
    {
        // Arrange
        var userId = SeedUser("Honza", UserRoles.User);

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(userId, "Honza", null, UserRoles.Admin))
        };
        request.Headers.Add("X-User-Id", userId.ToString());
        request.Headers.Add("X-User-Role", UserRoles.User);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

        var unchanged = await GetUserAsync(userId);
        Assert.Equal(UserRoles.User, unchanged.Role);
    }

    [Fact]
    public async Task TC18_UpdateUser_UserAttemptsToModifyAnotherUserProfile_Returns403Forbidden()
    {
        // Arrange
        var userA = SeedUser("User A", UserRoles.User);
        var userB = SeedUser("User B", UserRoles.User);

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(userB, "Hacked User B", null, UserRoles.User))
        };
        // Authenticated as User A
        request.Headers.Add("X-User-Id", userA.ToString());
        request.Headers.Add("X-User-Role", UserRoles.User);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

        var userBDetail = await GetUserAsync(userB);
        Assert.Equal("User B", userBDetail.Name);
    }

    [Fact]
    public async Task TC19_UpdateUser_AdminModifiesAnotherUserProfileAndChangesRole_Returns200()
    {
        // Arrange
        var userId = SeedUser("Target User", UserRoles.User);

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(userId, "Promoted User", null, UserRoles.Admin))
        };
        request.Headers.Add("X-User-Role", UserRoles.Admin);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updated = await GetUserAsync(userId);
        Assert.Equal("Promoted User", updated.Name);
        Assert.Equal(UserRoles.Admin, updated.Role);
    }

    [Fact]
    public async Task TC20_UpdateUser_UnknownId_Returns404NotFound()
    {
        // Arrange
        var unknownId = Guid.NewGuid();
        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(unknownId, "Ghost User", null, UserRoles.User))
        };
        request.Headers.Add("X-User-Role", UserRoles.Admin);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task TC21_UpdateUser_InvalidData_Returns422()
    {
        // Arrange
        var userId = SeedUser("Valid Name", UserRoles.User);

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(userId, "", null, UserRoles.User))
        };
        request.Headers.Add("X-User-Id", userId.ToString());
        request.Headers.Add("X-User-Role", UserRoles.User);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        await AssertValidationErrorAsync(response, "Name");
    }

    // ==========================================
    // 5. Deleting a User (DELETE /api/user/{id})
    // ==========================================

    [Fact]
    public async Task TC22_DeleteUser_AdminDeletesUserWithoutDependencies_Returns200AndUserIsRemoved()
    {
        // Arrange
        var userId = SeedUser("To Be Deleted", UserRoles.User);

        using var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/user/{userId}");
        request.Headers.Add("X-User-Role", UserRoles.Admin);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var getResponse = await Client.GetAsync($"/api/user/{userId}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task TC23_DeleteUser_RegularUserAttemptsToDeleteUser_Returns403Forbidden()
    {
        // Arrange
        var userA = SeedUser("User A", UserRoles.User);
        var userB = SeedUser("User B", UserRoles.User);

        // Act 1: Deleting someone else's account
        using var requestOther = new HttpRequestMessage(HttpMethod.Delete, $"/api/user/{userB}");
        requestOther.Headers.Add("X-User-Id", userA.ToString());
        requestOther.Headers.Add("X-User-Role", UserRoles.User);
        var responseOther = await Client.SendAsync(requestOther);

        // Assert 1
        Assert.Equal(HttpStatusCode.Forbidden, responseOther.StatusCode);

        // Act 2: Deleting your account
        using var requestSelf = new HttpRequestMessage(HttpMethod.Delete, $"/api/user/{userA}");
        requestSelf.Headers.Add("X-User-Id", userA.ToString());
        requestSelf.Headers.Add("X-User-Role", UserRoles.User);
        var responseSelf = await Client.SendAsync(requestSelf);

        // Assert 2
        Assert.Equal(HttpStatusCode.Forbidden, responseSelf.StatusCode);

        // Both users still exist
        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync($"/api/user/{userA}")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync($"/api/user/{userB}")).StatusCode);
    }

    [Fact]
    public async Task TC24_DeleteUser_AdminDeletesUnknownId_Returns404NotFound()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/user/{Guid.NewGuid()}");
        request.Headers.Add("X-User-Role", UserRoles.Admin);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task TC25_DeleteUser_UserWithRelatedEntities_PreservesReferentialIntegrity()
    {
        // Arrange: User owns a place
        var userId = SeedUser("Author User", UserRoles.User);
        var adminId = SeedUser("Admin User", UserRoles.Admin);
        var placeId = SeedPlace("Kitchen Shelf");

        Factory.Seed(db =>
        {
            var place = db.Places.Single(p => p.Id == placeId);
            place.OwnerId = userId.ToString();
        });

        // Act: Admin deletes user
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/user/{userId}");
        request.Headers.Add("X-User-Role", UserRoles.Admin);
        request.Headers.Add("X-User-Id", adminId.ToString());
        var response = await Client.SendAsync(request);

        // Assert: Delete was successful
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // User does not exist
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync($"/api/user/{userId}")).StatusCode);

        // Place DOES still exist and ownership was transferred to the deleting admin
        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync($"/api/place/{placeId}")).StatusCode);

        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var place = await db.Places.AsNoTracking().SingleAsync(p => p.Id == placeId);
        Assert.Equal(adminId.ToString(), place.OwnerId);
    }

    // ==========================================
    // 6. Authentication, Persistence, and Integration
    // ==========================================

    [Fact]
    public async Task TC26_Security_AnonymousCallsToModifyingEndpoints_Returns403Or401()
    {
        // Arrange
        var userId = SeedUser("Target", UserRoles.User);

        // Act 1: PUT without headers
        using var putRequest = new HttpRequestMessage(HttpMethod.Put, "/api/user")
        {
            Content = JsonContent.Create(new UserDetailModel(userId, "Anonymous Change", null, UserRoles.User))
        };
        var putResponse = await Client.SendAsync(putRequest);

        // Act 2: DELETE without headers
        using var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/user/{userId}");
        var deleteResponse = await Client.SendAsync(deleteRequest);

        // Assert
        Assert.Contains(putResponse.StatusCode, new[] { HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized });
        Assert.Contains(deleteResponse.StatusCode, new[] { HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized });
    }

    [Fact]
    public async Task TC27_Headers_MultipleRolesWithSpaces_ParsedCorrectlyAsAdmin()
    {
        // Arrange
        var userId = SeedUser("User To Remove", UserRoles.User);

        using var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/user/{userId}");
        // Header contains a comma and spaces
        request.Headers.Add("X-User-Role", "user, admin");

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TC28_Persistence_DataCreatedInSqlite_PersistsAcrossScopes()
    {
        // Arrange & Act: Creating a User via the API
        var createdId = await CreateUserViaApiAsync("Persistent User", UserRoles.User, "https://example.com/p.jpg");

        // Assert: Opening a new database scope and verifying that the data is stored in SQLite
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        var userInDb = await db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == createdId);
        Assert.NotNull(userInDb);
        Assert.Equal("Persistent User", userInDb.Name);
        Assert.Equal("https://example.com/p.jpg", userInDb.PhotoUrl);
        Assert.Equal(UserRoles.User, userInDb.Role);
    }

    [Fact]
    public async Task TC29_Domain_CreatePlaceWithUserIdHeader_AssignsOwnerIdToEntity()
    {
        // Arrange
        var authorId = Guid.NewGuid();

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/place")
        {
            Content = JsonContent.Create(new PlaceDetailModel { Name = "Living Room", ImageUrl = string.Empty })
        };
        request.Headers.Add("X-User-Id", authorId.ToString());
        request.Headers.Add("X-User-Role", UserRoles.User);

        // Act
        var response = await Client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdPlaceId = await response.Content.ReadFromJsonAsync<Guid>();

        // Assert: Verifying the OwnerId setting in the database
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        var place = await db.Places.AsNoTracking().SingleOrDefaultAsync(p => p.Id == createdPlaceId);
        Assert.NotNull(place);
        Assert.Equal(authorId.ToString(), place.OwnerId);
    }

    [Fact]
    public async Task TC30_Domain_RegularUserCannotEditOthersPlace_Returns403()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var placeId = SeedPlace("Owner's Pantry");

        Factory.Seed(db =>
        {
            var p = db.Places.Single(x => x.Id == placeId);
            p.OwnerId = ownerId.ToString();
        });

        // Act: Another user (a stranger) will try to change the name of the place
        var strangerId = Guid.NewGuid();
        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/place")
        {
            Content = JsonContent.Create(new PlaceDetailModel
            {
                Id = placeId,
                Name = "Hacked Pantry Name",
                ImageUrl = string.Empty
            })
        };
        request.Headers.Add("X-User-Id", strangerId.ToString());
        request.Headers.Add("X-User-Role", UserRoles.User);

        var response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task TC31_DeleteUser_TransfersPlacesItemsAndShoppingListsToAdmin()
    {
        // Arrange
        var userId = SeedUser("User With All", UserRoles.User);
        var adminId = SeedUser("Admin Collector", UserRoles.Admin);
        var placeId = SeedPlace("Basement");
        var itemId = SeedItem("Flour");
        var listId = SeedShoppingList("Party Supplies", "Metro");

        Factory.Seed(db =>
        {
            var p = db.Places.Single(x => x.Id == placeId);
            p.OwnerId = userId.ToString();

            var i = db.Items.Single(x => x.Id == itemId);
            i.OwnerId = userId.ToString();

            var l = db.ShoppingLists.Single(x => x.Id == listId);
            l.OwnerId = userId.ToString();
        });

        // Act: Admin deletes user
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/user/{userId}");
        request.Headers.Add("X-User-Role", UserRoles.Admin);
        request.Headers.Add("X-User-Id", adminId.ToString());
        var response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        // User is deleted
        Assert.Null(await db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == userId));

        // Place, item, and shopping list are all transferred to adminId
        var place = await db.Places.AsNoTracking().SingleAsync(p => p.Id == placeId);
        Assert.Equal(adminId.ToString(), place.OwnerId);

        var item = await db.Items.AsNoTracking().SingleAsync(i => i.Id == itemId);
        Assert.Equal(adminId.ToString(), item.OwnerId);

        var list = await db.ShoppingLists.AsNoTracking().SingleAsync(l => l.Id == listId);
        Assert.Equal(adminId.ToString(), list.OwnerId);
    }
}