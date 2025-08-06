using TradeSignalManager.Core.Interfaces;
using TradeSignalManager.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add after CreateBuilder()
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddControllers();

builder.Services.AddScoped<ITradeSignalRepository, InMemoryTradeSignalRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors();
app.MapControllers();
app.Run();

