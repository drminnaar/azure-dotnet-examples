namespace ProductManagerFncAppV5.Data
{
    internal sealed record ProductImage
    {
        public ProductImage(string imageName)
        {
            var tokens = imageName.Split("-");
            ProductId = tokens[1].Remove(tokens[1].IndexOf("."));
            Department = tokens[0];
        }

        public ProductImage(string productId, string department)
        {
            ProductId = productId;
            Department = department;
        }

        public void Deconstruct(out string productId, out string department, out string name)
        {
            productId = ProductId;
            department = Department;
            name = Name;
        }

        public string ProductId { get; private init; } = string.Empty;
        public string Department { get; private init; } = string.Empty;

        public string Name => $"{Department.ToLower()}-{ProductId}.jpg";

        public static ProductImage From(string imageName) => new(imageName);

        public static ProductImage From(string productId, string department)
            => new(productId, department);
    }
}
