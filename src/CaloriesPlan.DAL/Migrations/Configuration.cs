using System.Data.Entity.Migrations;
using System.Linq;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using CaloriesPlan.DAL.DataModel;
using CaloriesPlan.DAL.Dao.EF;

namespace CaloriesPlan.DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<CaloriesPlan.DAL.AppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CaloriesPlan.DAL.AppContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var roleNames = new[] { "Admin", "Manager", "User" };
            foreach (var roleName in roleNames)
            {
                if (!context.Roles.Any(r => r.Name == roleName))
                {
                    var role = new IdentityRole { Name = roleName };
                    roleManager.Create(role);
                }
            }

            var userDao = new EFUserDao();

            var userNames = new[] { "Alex", "Mike", "Ulia" };
            for (int userIndex = 0; userIndex < userNames.Length; userIndex++)
            {
                var userName = userNames[userIndex];
                var roleName = roleNames[userIndex];

                var user = context.Users.FirstOrDefault(u => u.UserName == userName);
                if (user == null)
                {
                    user = new User { UserName = userName, DailyCaloriesLimit = 50 };

                    userDao.CreateUserAsync(user, "123456");
                }

                if (!context.Users.Any(u => u.UserName == userName && u.Roles.Any(ur => context.Roles.Any(r => r.Id == ur.RoleId && r.Name == roleName))))
                {
                    userDao.AddUserRoleAsync(user, roleName);
                }
            }
        }
    }
}
