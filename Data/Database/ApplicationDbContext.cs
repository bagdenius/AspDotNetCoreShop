using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Database
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        new public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = Guid.Parse("f477d305-d208-425c-a998-2039921bb8de").ToString(),
                    Name = "Action",
                    DisplayOrder = 1
                },
                new Category
                {
                    Id = Guid.Parse("950c60da-0ebd-4343-887b-b4dd178f6a29").ToString(),
                    Name = "Sci-Fi",
                    DisplayOrder = 2
                },
                new Category
                {
                    Id = Guid.Parse("d9e57bb3-8446-4e7d-9243-6b3b52010680").ToString(),
                    Name = "History",
                    DisplayOrder = 3
                },
                new Category
                {
                    Id = Guid.Parse("89cd8a8c-50df-4cf5-a593-16de5813d6aa").ToString(),
                    Name = "Horror",
                    DisplayOrder = 4
                });

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = Guid.Parse("b7e9b11f-f628-432c-9a31-6dc128ce5de3").ToString(),
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SWD9999001",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = Guid.Parse("f477d305-d208-425c-a998-2039921bb8de").ToString(),
                    ImageUrl = ""
                },
                new Product
                {
                    Id = Guid.Parse("21626bfb-96f2-4dbb-84b1-d9bf7b8b8d95").ToString(),
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = Guid.Parse("950c60da-0ebd-4343-887b-b4dd178f6a29").ToString(),
                    ImageUrl = ""
                },
                new Product
                {
                    Id = Guid.Parse("0fcbc886-dd0e-49fb-9fd9-cd5c647a9d98").ToString(),
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "RITO5555501",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId = Guid.Parse("d9e57bb3-8446-4e7d-9243-6b3b52010680").ToString(),
                    ImageUrl = ""
                },
                new Product
                {
                    Id = Guid.Parse("2bd0613e-1e6f-411f-8243-a0c490d6743e").ToString(),
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WS3333333301",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = Guid.Parse("89cd8a8c-50df-4cf5-a593-16de5813d6aa").ToString(),
                    ImageUrl = ""
                },
                new Product
                {
                    Id = Guid.Parse("49758f0d-774a-481f-8647-48a2845b4ebe").ToString(),
                    Title = "Rock in the Ocean",
                    Author = "Ron Parker",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SOTJ1111111101",
                    ListPrice = 30,
                    Price = 27,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = Guid.Parse("f477d305-d208-425c-a998-2039921bb8de").ToString(),
                    ImageUrl = ""
                },
                new Product
                {
                    Id = Guid.Parse("0767fb28-f5af-4a42-84e7-3c1d503efd80").ToString(),
                    Title = "Leaves and Wonders",
                    Author = "Laura Phantom",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "FOT000000001",
                    ListPrice = 25,
                    Price = 23,
                    Price50 = 22,
                    Price100 = 20,
                    CategoryId = Guid.Parse("89cd8a8c-50df-4cf5-a593-16de5813d6aa").ToString(),
                    ImageUrl = ""
                });

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = Guid.Parse("df4bbe9f-b303-4c04-86cb-128c7f0b4ac9").ToString(),
                    Name = "Masonic Int",
                    PhoneNumber = "+1(406)-564-6357",
                    Country = "United States",
                    State = "Iowa",
                    City = "Allerton",
                    Address = "17 Masonic Drive",
                    PostalCode = "50008"
                },
                new Company
                {
                    Id = Guid.Parse("8c31a04a-ded0-47bd-9005-eb62cbc7a22f").ToString(),
                    Name = "Neuport Lane Tech",
                    PhoneNumber = "+1(770)-312-8562",
                    Country = "United States",
                    State = "Georgia",
                    City = "Duluth",
                    Address = "3332 Neuport Lane",
                    PostalCode = "30097"
                },
                new Company
                {
                    Id = Guid.Parse("e2f94a3f-11dc-4bfd-ae1b-cfc7672706b6").ToString(),
                    Name = "Roy Alley Co.",
                    PhoneNumber = "+1(303)-865-1479",
                    Country = "United States",
                    State = "Colorado",
                    City = "Greenwood Village",
                    Address = "4279 Roy Alley",
                    PostalCode = "80111"
                },
                new Company
                {
                    Id = Guid.Parse("a5cdcf9c-a679-4498-905b-3104538ede0c").ToString(),
                    Name = "Hall Place GmBH",
                    PhoneNumber = "+1(903)-674-5068",
                    Country = "United States",
                    State = "Texas",
                    City = "Detroit",
                    Address = "3961 Hall Place",
                    PostalCode = "75436"
                },
                new Company
                {
                    Id = Guid.Parse("0a43b720-b7b4-4e56-9f20-0968c1f1e73c").ToString(),
                    Name = "DyeS Chandler Co.",
                    PhoneNumber = "+1(480)-782-1697",
                    Country = "United States",
                    State = "Arizona",
                    City = "Chandler",
                    Address = "3173 Dye Street",
                    PostalCode = "85225"
                });
        }
    }
}
