using CaloriesPlan.DAL.DataModel.Abstractions;

namespace CaloriesPlan.DAL.DataModel
{
    public class Role : IRole
    {
        public string Name { get; set; }

        public Role(string roleName)
        {
            this.Name = roleName;
        }
    }
}
