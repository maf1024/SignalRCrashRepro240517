using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

ISignalRServerBuilder signalRBuilder = builder.Services.AddSignalR(
    hubOptions =>
    {
        hubOptions.MaximumReceiveMessageSize = 32 * 1024 * 1024;
        hubOptions.SupportedProtocols?.Remove("json");
    })
    .AddMessagePackProtocol();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<PresentsHub>("/Presents");

app.MapControllers();

app.Run();
