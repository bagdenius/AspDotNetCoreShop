using AspNetCoreCourseProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreCourseProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = Guid.Parse("10CD0351-3271-46E9-90FE-0BD3E5F9C214"),
                    Name = "Action",
                    DisplayOrder = 1
                },
                new Category
                {
                    Id = Guid.Parse("151EADBB-17DB-43A4-B7AB-9DE78EB24C15"),
                    Name = "Sci-Fi",
                    DisplayOrder = 2
                },
                new Category
                {
                    Id = Guid.Parse("EFE086E6-E51A-4EE9-BD8D-CAF2C60F55A1"),
                    Name = "History",
                    DisplayOrder = 3
                });
        }
    }
}
