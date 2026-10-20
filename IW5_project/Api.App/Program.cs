using IW5_project.Api.DAL.Common.Queries;
using IW5_project.Api.DAL.Common.Repositories;
using IW5_project.API.BL.Mappers;
using IW5_project.API.Common.Models.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Temporary Database registration
builder.Services.AddSingleton<AppDbContext>();

builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<TransferObjects>();

builder.Services.AddTransient<ItemFinder>();
builder.Services.AddTransient<OrderItems>();
builder.Services.AddTransient<FilterItems>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();