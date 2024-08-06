using Data.Database;
using Data.DbInitializer.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Utility;

namespace Data.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {

            // push migration if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception e) { }

            // create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();

                // if roles are not created, then we'll create admin user as well
                _userManager.CreateAsync(new User
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    Name = "Bohdan",
                    Surname = "Bondarenko",
                    PhoneNumber = "0687007132",
                    Country = "Ukraine",
                    State = "Cherkaska oblast",
                    City = "Cherkasy",
                    Address = "Sumhaitska st., b. 37, apt. 40",
                    PostalCode = "18029"
                }, "admin").GetAwaiter().GetResult();
                User user = _db.Users.FirstOrDefault(u => u.Email == "admin@admin.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
        }
    }
}
