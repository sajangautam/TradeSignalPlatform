using Microsoft.EntityFrameworkCore;
using TradeSignalManager.Infrastructure;
using TradeSignalManager.Core.Interfaces;
using TradeSignalManager.Infrastructure.Repositories;
using TradeSignalManager.Infrastructure.Data;
using TradeSignalManager.Infrastructure.Services;




var builder = WebApplication.CreateBuilder(args);

// Add after CreateBuilder()

builder.Services.AddHttpClient();
builder.Services.AddHttpClient<AlphaVantageService>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:30000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();

builder.Services.AddScoped<ITradeSignalRepository, EfCoreTradeSignalRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated(); // Make sure DB exists
    SeedSp500.Seed(context); // Run the seeding
}

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowFrontend");
app.MapControllers();
app.Run();

