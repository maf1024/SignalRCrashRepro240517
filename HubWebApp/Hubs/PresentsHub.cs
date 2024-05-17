using Microsoft.AspNetCore.SignalR;

public class PresentsHub : Hub
{
    public async Task Marco(byte[] payload)
    {
        await Clients.All.SendAsync("Polo", payload);
    }
}