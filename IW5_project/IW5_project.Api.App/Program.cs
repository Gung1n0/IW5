using FluentValidation;
using IW5_project.Api.BL.Facades;
using IW5_project.Api.Common;
using IW5_project.Api.Common.Models.Common;
using IW5_project.Api.DAL.Common.Repositories;
using IW5_project.Api.DAL.EF;                      
using IW5_project.Api.DAL.EF.Repositories;
using IW5_project.Api.App.Extensions;
using IW5_project.Common.Models.Place;
using Microsoft.EntityFrameworkCore;
using BlPlaceMapper = IW5_project.Api.BL.Mappers.PlaceMapper;
using DalPlaceMapper = IW5_project.Api.DAL.Common.Mappers.PlaceMapper;

var builder = WebApplication.CreateBuilder(args);

// keep default OpenAPI / Swagger service lines here

builder.Services.AddHttpContextAccessor();

// database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new ArgumentException("The connection string is missing");

// TODO: change to database provider
//builder.Services.AddDbContext<DatabaseContext>(options =>
//    options.UseSqlite(connectionString));

// DAL
builder.Services.AddSingleton<DalPlaceMapper>();
builder.Services.AddScoped<IPlaceRepository, PlaceRepository>();

// BL
builder.Services.AddSingleton<BlPlaceMapper>();
builder.Services.AddScoped<IPlaceFacade, PlaceFacade>();

// validation
builder.Services.AddScoped<IValidator<PlaceDetailModel>, PlaceDetailModelValidator>();
builder.Services.AddScoped<IValidator<PlaceQuery>, PlaceQueryValidator>();

var app = builder.Build();

// keep your default OpenAPI / Swagger middleware lines here
app.UseHttpsRedirection();

var api = app.MapGroup("api");
api.UsePlaceEndpoints();

app.Run();

public partial class Program;                      // lets the end-to-end tests use WebApplicationFactory<Program>