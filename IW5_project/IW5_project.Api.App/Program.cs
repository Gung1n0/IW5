using FluentValidation;
using IW5_project.Api.App.Extensions;
using IW5_project.Api.BL.Facades;
using IW5_project.Api.DAL.Common.Repositories;
using IW5_project.Api.DAL.EF;
using IW5_project.Api.DAL.EF.Repositories;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.Place;
using IW5_project.Common.Models.User;
using Microsoft.EntityFrameworkCore;
using BlPlaceMapper = IW5_project.Api.BL.Mappers.PlaceMapper;
using DalPlaceMapper = IW5_project.Api.DAL.Common.Mappers.PlaceMapper;
using BlUserMapper = IW5_project.Api.BL.Mappers.UserMapper;
using DalUserMapper = IW5_project.Api.DAL.Common.Mappers.UserMapper;

var builder = WebApplication.CreateBuilder(args);

// keep default OpenAPI / Swagger service lines here

builder.Services.AddHttpContextAccessor();

// database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new ArgumentException("The connection string is missing");

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(connectionString));

// DAL
builder.Services.AddSingleton<DalPlaceMapper>();
builder.Services.AddSingleton<DalUserMapper>();
builder.Services.AddScoped<IPlaceRepository, PlaceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// BL
builder.Services.AddSingleton<BlPlaceMapper>();
builder.Services.AddSingleton<BlUserMapper>();
builder.Services.AddScoped<IPlaceFacade, PlaceFacade>();
builder.Services.AddScoped<IUserFacade, UserFacade>();

// validation
builder.Services.AddScoped<IValidator<PlaceDetailModel>, PlaceDetailModelValidator>();
builder.Services.AddScoped<IValidator<PlaceQuery>, PlaceQueryValidator>();
builder.Services.AddScoped<IValidator<UserDetailModel>, UserDetailModelValidator>();
builder.Services.AddScoped<IValidator<UserQuery>, UserQueryValidator>();

var app = builder.Build();

// keep your default OpenAPI / Swagger middleware lines here
app.UseHttpsRedirection();

var api = app.MapGroup("api");
api.UsePlaceEndpoints();
api.UseUserEndpoints();

app.Run();

namespace IW5_project.Api.App
{
    public partial class Program;                      // lets the end-to-end tests use WebApplicationFactory<Program>
}