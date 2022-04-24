using System;
using System.Collections.Generic;

namespace ProductManagerFncAppV5.Models;

internal sealed record ProductForCreateOrUpdate
{
    public IEnumerable<string> Categories { get; init; } = Array.Empty<string>();
    public string Department { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Title { get; init; } = string.Empty;
    public string ImageContent { get; init; } = string.Empty;
}
