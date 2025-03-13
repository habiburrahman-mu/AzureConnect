using StackExchange.Redis;

var connectionString = "your-redis-connection-string";

using (var cache = ConnectionMultiplexer.Connect(connectionString))
{
    IDatabase database = cache.GetDatabase();

    var result = await database.ExecuteAsync("PING");
    Console.WriteLine($"PING = {result.Resp2Type}: {result}");
    /// PING = SimpleString : PONG

    string key = "mykey";
    bool setValue = await database.StringSetAsync(key, "myvalue");
    Console.WriteLine($"SET {key} = {setValue}");
    /// SET mykey = True

    var getValue = await database.StringGetAsync(key);
    Console.WriteLine($"GET {key} = {getValue}");
    /// GET mykey = myvalue
}