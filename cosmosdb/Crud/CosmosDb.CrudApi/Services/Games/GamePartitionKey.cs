using Azure.Cosmos;

namespace CosmosDb.CrudApi.Services.Games;

public record struct GamePartitionKey
{
    private readonly PartitionKey _partitionKey;
    private readonly string _paritionKeyValue;

    public GamePartitionKey(GameForCreate game)
    {
        _paritionKeyValue = DeterminePartitionKey(game.Platform);
        _partitionKey = new(DeterminePartitionKey(game.Platform));
    }

    public GamePartitionKey(GameForUpdate game)
    {
        _paritionKeyValue = DeterminePartitionKey(game.Platform);
        _partitionKey = new(DeterminePartitionKey(game.Platform));
    }

    public GamePartitionKey(string platform)
    {
        _paritionKeyValue = DeterminePartitionKey(platform);
        _partitionKey = new(DeterminePartitionKey(platform));
    }

    //public PartitionKey Value => _partitionKey;

    public override string ToString() => _paritionKeyValue;

    private static string DeterminePartitionKey(string platform) =>
        $"{GameEntity.EntityType.ToLowerInvariant()}-{platform.ToLowerInvariant()}";

    public static GamePartitionKey Create(GameForCreate game) => new(game);

    public static GamePartitionKey Create(GameForUpdate game) => new(game);

    public static GamePartitionKey Create(string platform) => new(platform);

    public static PartitionKey Value(GameForCreate game) => Create(game)._partitionKey;

    public static PartitionKey Value(GameForUpdate game) => Create(game)._partitionKey;

    public static PartitionKey Value(string platform) => Create(platform)._partitionKey;
}
