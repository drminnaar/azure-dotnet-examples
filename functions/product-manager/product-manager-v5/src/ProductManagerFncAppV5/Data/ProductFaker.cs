using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using ProductManagerFncAppV5.Data.Models;

namespace ProductManagerFncAppV5.Data;

internal interface IProductFaker
{
    IReadOnlyCollection<ProductEntity> GenerateRandomProducts(int count);
}

internal class ProductFaker : IProductFaker
{
    public IReadOnlyCollection<ProductEntity> GenerateRandomProducts(int count)
    {
        Randomizer.Seed = new Random(8675309);

        return new Faker<ProductEntity>()
            .RuleFor(r => r.Department, (f, r) => f.Commerce.Department(1))
            .RuleFor(r => r.Pk, (f, r) => $"{r.Department.ToLower()}")
            .RuleFor(r => r.Id, (f, r) => f.Random.Hash())
            .RuleFor(r => r.Categories, (f, r) => DetermineCategories(f))
            .RuleFor(r => r.Description, (f, r) => f.Lorem.Paragraph())
            .RuleFor(r => r.Price, (f, r) => decimal.Parse(f.Commerce.Price(1, 100, 2)))
            .RuleFor(r => r.Title, (f, r) => f.Commerce.ProductName())
            .RuleFor(r => r.Type, (f, r) => "product")
            .RuleFor(r => r.ImageUrl, (f, r) => f.Image.PicsumUrl(450, 450))
            .RuleFor(r => r.ImageMediumUrl, (f, r) => f.Image.PicsumUrl(250, 250))
            .RuleFor(r => r.ImageExtraSmallUrl, (f, r) => f.Image.PicsumUrl(50, 50))
            .RuleFor(r => r.ImageSmallUrl, (f, r) => f.Image.PicsumUrl(150, 150))
            .RuleFor(r => r.CreatedAt, (f, r) => f.Date.PastOffset(1).UtcDateTime)
            .RuleFor(r => r.UpdatedAt, (f, r) => r.CreatedAt)
            .Generate(count);
    }

    private static List<string> DetermineCategories(Faker faker) =>
        faker.Commerce
            .Categories(faker.Random.Number(1, 5))
            .Select(category => category.ToLower())
            .ToList();
}
