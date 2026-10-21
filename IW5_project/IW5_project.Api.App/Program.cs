using FluentValidation;
using IW5_project.Api.App.Extensions;
using IW5_project.Api.BL.Facades;
using IW5_project.Api.Common.Models.Common;
using IW5_project.Api.DAL.Common.Queries;
using IW5_project.Api.DAL.Common.Repositories;
using IW5_project.Api.DAL.EF;
using IW5_project.Api.DAL.EF.Repositories;
using IW5_project.API.Common.Models.Common;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.Place;
using IW5_project.Common.Models.ShoppingList;
using Microsoft.EntityFrameworkCore;
using BlPlaceMapper = IW5_project.Api.BL.Mappers.PlaceMapper;
using BlShoppingListMapper = IW5_project.Api.BL.Mappers.ShoppingListMapper;
using DalPlaceMapper = IW5_project.Api.DAL.Common.Mappers.PlaceMapper;
using DalShoppingListMapper = IW5_project.Api.DAL.Common.Mappers.ShoppingListMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();

builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<ItemFinder>();
builder.Services.AddScoped<OrderItems>();
builder.Services.AddScoped<FilterItems>();

// database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new ArgumentException("The connection string is missing");

// TODO: change to database provider
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(connectionString));

// DAL
builder.Services.AddSingleton<DalPlaceMapper>();
builder.Services.AddSingleton<DalShoppingListMapper>();
builder.Services.AddScoped<IPlaceRepository, PlaceRepository>();
builder.Services.AddScoped<IShoppingListRepository, ShoppingListRepository>();

// BL
builder.Services.AddSingleton<BlPlaceMapper>();
builder.Services.AddSingleton<BlShoppingListMapper>();
builder.Services.AddScoped<IPlaceFacade, PlaceFacade>();
builder.Services.AddScoped<IShoppingListFacade, ShoppingListFacade>();

// validation
ConfigureValidation(builder.Services);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseOpenApi();
app.UseSwaggerUi();

var api = app.MapGroup("api");
api.UsePlaceEndpoints();
api.UseShoppingListEndpoints();

app.Run();

public partial class Program                       // lets the end-to-end tests use WebApplicationFactory<Program>
{
    private static void ConfigureValidation(IServiceCollection services)
    {
        // place
        services.AddScoped<IValidator<PlaceDetailModel>, PlaceDetailModelValidator>();
        services.AddScoped<IValidator<PlaceQuery>, PlaceQueryValidator>();

        // shopping list
        services.AddScoped<IValidator<ShoppingListDetailModel>, ShoppingListDetailModelValidator>();
        services.AddScoped<IValidator<ShoppingListQuery>, ShoppingListQueryValidator>();
        services.AddScoped<IValidator<AddShoppingListItemModel>, AddShoppingListItemModelValidator>();
        services.AddScoped<IValidator<UpdateShoppingListItemModel>, UpdateShoppingListItemModelValidator>();
    }
}