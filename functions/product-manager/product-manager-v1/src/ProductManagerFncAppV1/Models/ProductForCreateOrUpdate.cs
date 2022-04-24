using System.Collections.Generic;
using System.Linq;

namespace ProductManagerFncAppV1.Models;

internal sealed record ProductForCreateOrUpdate
{
    public string Category { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Title { get; init; } = string.Empty;
}
