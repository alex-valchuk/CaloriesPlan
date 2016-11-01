namespace CaloriesPlan.DAL.Dao.EF.Base
{
    public abstract class EFDaoBase
    {
        protected readonly AppContext dbContext;

        public EFDaoBase()
        {
            this.dbContext = new AppContext();
        }
    }
}
