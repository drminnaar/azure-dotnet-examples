using System;

namespace Azure.Cosmos;

public interface ICosmosEntityIdProvider
{
    string GenerateId();
}

public sealed class CosmosEntityIdProvider : ICosmosEntityIdProvider
{
    public string GenerateId() => Guid.NewGuid().ToString();
}
