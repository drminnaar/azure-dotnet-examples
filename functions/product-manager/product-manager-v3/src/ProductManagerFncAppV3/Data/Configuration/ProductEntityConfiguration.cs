using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagerFncAppV3.Data.Models;

namespace ProductManagerFncAppV3.Data.Configuration;

internal sealed class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        builder.HasKey(e => e.Id);
    }
}
