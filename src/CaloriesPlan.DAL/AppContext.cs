using System.Data.Entity;

using Microsoft.AspNet.Identity.EntityFramework;

using CaloriesPlan.DAL.DataModel;

namespace CaloriesPlan.DAL
{
    public class AppContext : IdentityDbContext<User>
    {
        public AppContext()
            : base("AppContext")
        {
        }

        public AppContext(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<Meal> Meals { get; set; }
        public virtual DbSet<UserSubscription> Subscribtions { get; set; }
    }
}