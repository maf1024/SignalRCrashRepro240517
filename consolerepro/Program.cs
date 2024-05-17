using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;


Console.WriteLine("Hello, World!");

var headers = new Dictionary<string, string> { { "asdf", "qwerty" }, { "foo", "bar" } };
Func<Task<string?>>? access_token_provider = () => Task.FromResult<string?>(Guid.NewGuid().ToString());

var hub_connection = new HubConnectionBuilder()
            .WithUrl("https://[CHANGE THIS TO YOUR WEBAPP URL]/Presents", options =>
            {
                options.Transports = HttpTransportType.WebSockets;
                options.Headers = headers;
                options.AccessTokenProvider = access_token_provider;
            })
            .AddMessagePackProtocol()
            .WithAutomaticReconnect(new[]{
                TimeSpan.FromSeconds( 1 ),
                TimeSpan.FromSeconds( 1 ),
                TimeSpan.FromSeconds( 2 ),
                TimeSpan.FromSeconds( 2 )
        })
        .Build();

int highest_polo = -1;

hub_connection.On("Polo", (byte[] payload) => {
    highest_polo = Math.Max(highest_polo, payload.Length);
    if(payload.Any(b => b != 255)) throw new Exception("Invalid byte encountered");
    Console.WriteLine($"Polo {payload.Length} bytes");
});

await hub_connection.StartAsync();

const int START = 9900;
const int LIMIT = 10000;
for(int i = START; i < LIMIT; i++)
{
    Console.WriteLine($"Marco {i}");
    byte[] array = new byte[i];
    Array.Fill(array, (byte)255);
    await hub_connection.SendAsync("Marco", array);
}

DateTime start_wait = DateTime.UtcNow;
while(true)
{
    if(highest_polo == LIMIT - 1)
    {
        break;
    }
    await Task.Delay(TimeSpan.FromMilliseconds(100));
    if(DateTime.UtcNow - start_wait > TimeSpan.FromSeconds(30))
    {
        throw new Exception("Waited too long for last response");
    }
}
Console.WriteLine($"Done!");