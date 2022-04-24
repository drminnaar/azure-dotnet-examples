using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductManagerFncAppV2.Data.Models;

namespace ProductManagerFncAppV2.Data
{
    internal sealed class Seeder
    {
        private readonly InventoryDbContext _context;

        public Seeder(InventoryDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            SeedProducts();
            _context.SaveChanges();
        }

        public async Task SeedAsync()
        {
            SeedProducts();
            await _context.SaveChangesAsync();
        }

        private void SeedProducts()
        {
            if (_context.Products.Any())
                return;

            #region Product Data
            var products = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Id = "e93e7f02-7796-474f-8a5d-4474078db150",
                    Category = "Electronics",
                    Description = "Est id elit sit ad elit dolor consequat esse eiusmod sunt do exercitation sint. Reprehenderit non mollit nostrud veniam aliquip mollit magna. Excepteur enim laboris consectetur commodo culpa tempor magna aute ullamco labore eiusmod ad dolor.",
                    Price = 603.44M,
                    Title = "Unbranded Steel Salad"
                },
                new ProductEntity
                {
                    Id = "36eaccf5-7bc7-4a9d-a538-f9d4c2087402",
                    Category = "Music",
                    Description = "Velit enim architecto necessitatibus eaque in repellendus excepturi. Autem ut esse corporis est ut ut repellat numquam. Cum quis delectus officiis excepturi praesentium enim distinctio non. Alias mollitia repudiandae voluptas illo.",
                    Price = 970.22M,
                    Title = "Ergonomic Steel Cheese"
                },
                new ProductEntity
                {
                    Id = "5974a12d-1907-4d57-90d1-a416e1ddfcd4",
                    Category = "Industrial",
                    Description = "Numquam doloribus voluptatum qui. Voluptatibus sit nam dolore. Debitis aut sed dolores laborum ut itaque cupiditate deserunt. Est necessitatibus qui eligendi ut ea quasi.",
                    Price = 229.78M,
                    Title = "Sleek Fresh Shirt"
                },
                new ProductEntity
                {
                    Id = "7ec7da29-722a-4603-bb51-f4b5d5aaf5a9",
                    Category = "Sports",
                    Description = "Ducimus quia eaque ut consectetur odio. Corporis tempora sunt accusamus eum voluptatem dolorem. Ducimus quam voluptatibus qui id eos. Occaecati officia quia dolor excepturi debitis corporis pariatur.",
                    Price = 111.39M,
                    Title = "Sleek Soft Ball"
                },
                new ProductEntity
                {
                    Id = "2bab820b-ecff-4404-baae-905e55f47523",
                    Category = "Grocery",
                    Description = "Porro sed beatae culpa tempora. Atque aliquam sed aut assumenda quo commodi consequatur placeat. Ut et qui ipsa eius.",
                    Price = 514.87M,
                    Title = "Intelligent Rubber Fish"
                },
                new ProductEntity
                {
                    Id = "72459944-7470-466b-af24-e78ee2267769",
                    Category = "Garden",
                    Description = "Magni et qui necessitatibus. Blanditiis quaerat ratione voluptas culpa dignissimos culpa et quaerat. Quas et vero numquam sed. A excepturi sunt aut.",
                    Price = 909.76M,
                    Title = "Fantastic Metal Pizza"
                },
                new ProductEntity
                {
                    Id = "0198934c-efe0-4bc7-8383-d5489a1f359e",
                    Category = "Movies",
                    Description = "Quisquam unde voluptatem. Eos praesentium dolorem animi cupiditate qui rerum. Reprehenderit eos dolor cumque explicabo quisquam inventore tempore reprehenderit quibusdam. Reiciendis tenetur temporibus molestiae. Sapiente itaque corporis voluptatem eius corrupti repudiandae rerum rerum. Ipsum aut nihil et quia.",
                    Price = 372.03M,
                    Title = "Unbranded Plastic Gloves"
                },
                new ProductEntity
                {
                    Id = "abb11dee-b724-4ae7-b780-8e780b4a8710",
                    Category = "Computers",
                    Description = "Numquam aut doloribus iure rerum sed sapiente beatae autem. Non sunt sint voluptatem tempora dolorem quia cupiditate. Animi non nisi aut nihil minima aut quia distinctio ut. Qui quas occaecati et illum fugiat et beatae suscipit.",
                    Price = 538.30M,
                    Title = "Practical Metal Shirt"
                },
                new ProductEntity
                {
                    Id = "aa243a68-a377-4082-8a54-2287a30ededb",
                    Category = "Games",
                    Description = "Qui placeat vel. Et non ut et impedit impedit hic. Omnis maiores non voluptas quia labore delectus voluptatem voluptas. Quia quos quo quo. Culpa fugit quasi aperiam.",
                    Price = 75.26M,
                    Title = "Small Fresh Computer"
                },
                new ProductEntity
                {
                    Id = "e406fd28-a682-40cd-9ed9-c05ef1167fed",
                    Category = "Books",
                    Description = "Ea ab hic debitis ipsum illo provident. Et fugit ducimus natus similique. Dolorem repudiandae quod et omnis tempore nisi incidunt quia. Magnam accusantium incidunt reprehenderit libero mollitia exercitationem aspernatur dolorem. Magnam adipisci delectus pariatur et dolor est quo.",
                    Price = 398.00M,
                    Title = "Incredible Cotton Pants"
                },
                new ProductEntity
                {
                    Id = "cca33038-98e2-4422-90d7-eb901bc2a7d5",
                    Category = "Movies",
                    Description = "Provident voluptatem consequatur ex molestias autem exercitationem aliquid impedit. Iusto aut odio est eos ipsum omnis. Molestiae perferendis possimus. A ipsum temporibus similique perspiciatis sunt. Sunt ratione rerum est consequatur labore sapiente.",
                    Price = 195.91M,
                    Title = "Tasty Cotton Ball"
                },
                new ProductEntity
                {
                    Id = "471cdfda-ee85-4b1a-9597-f8478bd7c4fd",
                    Category = "Shoes",
                    Description = "Voluptas quisquam voluptate tenetur ut dolorem aut. Quo molestiae autem itaque qui enim voluptas consectetur. Autem qui omnis nostrum quia itaque. Impedit qui autem nostrum sint aut saepe eos doloremque.",
                    Price = 618.60M,
                    Title = "Handmade Rubber Soap"
                },
                new ProductEntity
                {
                    Id = "726613aa-804a-4049-97c8-2c4028ff5dfa",
                    Category = "Tools",
                    Description = "Ducimus qui quia aliquid eaque officia consequatur impedit perspiciatis et. Voluptates cum accusamus quidem et distinctio aliquam sed. Impedit similique qui et. Distinctio molestias sit deleniti ratione perferendis. Sunt sapiente eos architecto et.",
                    Price = 174.78M,
                    Title = "Practical Granite Bike"
                },
                new ProductEntity
                {
                    Id = "b801d1ff-a009-4e6c-ab62-41b78ca1e5b2",
                    Category = "Home",
                    Description = "Et aut velit magni ut magnam. Neque assumenda temporibus. Incidunt voluptas repellat modi qui sed.",
                    Price = 247.58M,
                    Title = "Refined Granite Chair"
                },
                new ProductEntity
                {
                    Id = "c26baa39-40fa-4bf9-b2ea-86b167a4f614",
                    Category = "Home",
                    Description = "Vitae expedita voluptas exercitationem et est velit consequatur. Deserunt omnis magnam rerum blanditiis temporibus illo ducimus. Officia ut ab omnis. Facere nam velit maxime quia. Sed id eligendi rerum qui necessitatibus velit consequatur. Quod exercitationem sit corporis alias saepe atque deleniti voluptas.",
                    Price = 532.35M,
                    Title = "Tasty Wooden Mouse"
                },
                new ProductEntity
                {
                    Id = "814d3950-3fda-41f8-bb71-37d9201abb76",
                    Category = "Shoes",
                    Description = "Eaque earum quibusdam soluta tempore. Illo dolorem eum. Et nostrum sunt nulla inventore dolore ab. In aut mollitia rerum cumque. Laboriosam repudiandae necessitatibus magni.",
                    Price = 950.92M,
                    Title = "Handmade Rubber Mouse"
                },
                new ProductEntity
                {
                    Id = "e56137ca-2f5a-4ecb-a5cd-3d52e648dc8b",
                    Category = "Grocery",
                    Description = "Nemo debitis ipsum ut officiis et deserunt maxime. Ea delectus nihil quae doloribus dolorem. Officia ullam dolores non aliquam. Laboriosam sed ab. Repellendus non rem autem ut molestias odit placeat in accusamus. Ut animi nihil sint aspernatur rem ducimus similique recusandae repellat.",
                    Price = 534.26M,
                    Title = "Refined Concrete Soap"
                },
                new ProductEntity
                {
                    Id = "494c589e-0f80-49d6-ab55-e7c48c438bc4",
                    Category = "Home",
                    Description = "Odit et similique officia enim nihil eveniet unde. Nisi quia consectetur voluptas dolorem sint eos et. Impedit consectetur explicabo.",
                    Price = 855.88M,
                    Title = "Sleek Concrete Car"
                },
                new ProductEntity
                {
                    Id = "7cf8a5d2-9d25-4770-97f6-0aca5ef4c444",
                    Category = "Clothing",
                    Description = "Ab repellendus culpa qui eum exercitationem voluptatum. Vel perspiciatis consequatur qui quisquam ut iure natus cumque. Iste et aperiam. Nihil est quia dolor et. Cum sunt cum sed atque. Voluptatem corrupti sunt accusantium molestias officia aut sint vero voluptatem.",
                    Price = 371.37M,
                    Title = "Generic Wooden Tuna"
                },
                new ProductEntity
                {
                    Id = "37cf4ffa-9385-4f05-90a5-c39dead509d1",
                    Category = "Games",
                    Description = "Exercitationem accusamus qui. Consectetur possimus praesentium dolorem sequi libero explicabo. Laborum beatae neque. Corrupti est ex aut cupiditate vel excepturi aut ut. Voluptates sunt cum perferendis accusantium porro. Nostrum minima aut fugiat eos doloremque.",
                    Price = 988.53M,
                    Title = "Intelligent Soft Soap"
                },
                new ProductEntity
                {
                    Id = "5780f72f-efad-42e8-8b74-a2aef94720fb",
                    Category = "Sports",
                    Description = "Ad quas nesciunt possimus. Laudantium quia atque veritatis voluptatibus eum et ipsa consequatur. Eaque fuga architecto corporis nesciunt deleniti alias voluptatem. Assumenda et aut animi dicta voluptatibus accusamus.",
                    Price = 992.86M,
                    Title = "Gorgeous Fresh Soap"
                },
                new ProductEntity
                {
                    Id = "5a0e1828-b968-4bf3-ac50-609f17234a8c",
                    Category = "Movies",
                    Description = "Necessitatibus blanditiis corporis. Quia accusamus rerum placeat ad nostrum architecto dolore sint sint. Earum voluptatem minus non.",
                    Price = 797.65M,
                    Title = "Generic Granite Shoes"
                },
                new ProductEntity
                {
                    Id = "abea9674-4bb8-45f2-927c-70ca7903aab6",
                    Category = "Outdoors",
                    Description = "Ex at ipsam. Cupiditate laudantium aut reprehenderit harum provident perspiciatis corrupti qui est. Est alias sed magni numquam molestias optio earum. Accusamus molestiae autem saepe.",
                    Price = 517.40M,
                    Title = "Unbranded Cotton Bacon"
                },
                new ProductEntity
                {
                    Id = "6c691f8e-a9a4-4693-bd9a-10062beb9f64",
                    Category = "Baby",
                    Description = "Nihil fugit unde reiciendis. Ad cumque rerum aperiam nemo odit natus facere tenetur ut. Sequi quisquam iusto placeat. Reiciendis eos ut quaerat sed ab sint possimus dolor.",
                    Price = 970.86M,
                    Title = "Generic Concrete Soap"
                },
                new ProductEntity
                {
                    Id = "2f6452aa-f337-4389-bad3-9ef8f5f7835f",
                    Category = "Toys",
                    Description = "Modi sed saepe accusantium sed eveniet vel quisquam. Sed vel velit nulla. Error voluptatem fugit ad. Repellendus omnis dolorem. Tempora eos ipsam.",
                    Price = 926.02M,
                    Title = "Handcrafted Fresh Ball"
                },
            };
            #endregion

            _context.Products.AddRange(products);
        }
    }
}
