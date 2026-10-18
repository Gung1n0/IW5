using FluentValidation;
using IW5_project.Api.App.Extensions;
using IW5_project.Api.BL.Facades;
using IW5_project.Api.DAL.Common.Repositories;
using IW5_project.Api.DAL.EF;
using IW5_project.Api.DAL.EF.Repositories;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.Place;
using IW5_project.Common.Models.ShoppingList;
using IW5_project.Common.Models.User;
using Microsoft.EntityFrameworkCore;
using BlPlaceMapper = IW5_project.Api.BL.Mappers.PlaceMapper;
using BlShoppingListMapper = IW5_project.Api.BL.Mappers.ShoppingListMapper;
using BlUserMapper = IW5_project.Api.BL.Mappers.UserMapper;
using DalPlaceMapper = IW5_project.Api.DAL.Common.Mappers.PlaceMapper;
using DalShoppingListMapper = IW5_project.Api.DAL.Common.Mappers.ShoppingListMapper;
using DalUserMapper = IW5_project.Api.DAL.Common.Mappers.UserMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();

// database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new ArgumentException("The connection string is missing");

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(connectionString));

// DAL
builder.Services.AddSingleton<DalPlaceMapper>();
builder.Services.AddSingleton<DalShoppingListMapper>();
builder.Services.AddSingleton<DalUserMapper>();
builder.Services.AddScoped<IPlaceRepository, PlaceRepository>();
builder.Services.AddScoped<IShoppingListRepository, ShoppingListRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// BL
builder.Services.AddSingleton<BlPlaceMapper>();
builder.Services.AddSingleton<BlShoppingListMapper>();
builder.Services.AddSingleton<BlUserMapper>();
builder.Services.AddScoped<IPlaceFacade, PlaceFacade>();
builder.Services.AddScoped<IShoppingListFacade, ShoppingListFacade>();
builder.Services.AddScoped<IUserFacade, UserFacade>();

// validation
IW5_project.Api.App.Program.ConfigureValidation(builder.Services);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseOpenApi();
app.UseSwaggerUi();

var api = app.MapGroup("api");
api.UsePlaceEndpoints();
api.UseShoppingListEndpoints();
api.UseUserEndpoints();

app.Run();

namespace IW5_project.Api.App
{
    public partial class Program
    {
        public static void ConfigureValidation(IServiceCollection services)
        {
            // place
            services.AddScoped<IValidator<PlaceDetailModel>, PlaceDetailModelValidator>();
            services.AddScoped<IValidator<PlaceQuery>, PlaceQueryValidator>();

            // shopping list
            services.AddScoped<IValidator<ShoppingListDetailModel>, ShoppingListDetailModelValidator>();
            services.AddScoped<IValidator<ShoppingListQuery>, ShoppingListQueryValidator>();
            services.AddScoped<IValidator<AddShoppingListItemModel>, AddShoppingListItemModelValidator>();
            services.AddScoped<IValidator<UpdateShoppingListItemModel>, UpdateShoppingListItemModelValidator>();

            // user
            services.AddScoped<IValidator<UserDetailModel>, UserDetailModelValidator>();
            services.AddScoped<IValidator<UserQuery>, UserQueryValidator>();
        }
    }
}