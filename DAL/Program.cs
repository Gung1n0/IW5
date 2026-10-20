using DAL;
using DAL.Mappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Temporary Database registration
builder.Services.AddSingleton<AppDbContext>();

builder.Services.AddScoped<PolozkaService>();
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