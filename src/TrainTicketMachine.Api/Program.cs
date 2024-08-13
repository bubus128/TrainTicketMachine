using TrainTicketMachine.Core;
using TrainTicketMachine.Core.Interfaces;
using TrainTicketMachine.Infrastructure.Interfaces;
using TrainTicketMachine.Infrastructure.Models;
using TrainTicketMachine.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient();

builder.Services.AddSingleton<ITrieRepository<TrieNode>, TrieRepository>();

builder.Services.AddSingleton<IStationsRepository<Station>, StationsRepository>();

builder.Services.AddSingleton<IStationService, StationService>();

builder.Services.AddHostedService<StationsDataInitializer>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
