using System.Transactions;

namespace CaloriesPlan.BLL.Services.Impl.Base
{
    public abstract class ServiceBase
    {
        protected TransactionScope CreateTransactionScope()
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.Serializable,
                Timeout = TransactionManager.MaximumTimeout,
                
            };

            return new TransactionScope(TransactionScopeOption.Required, options);
        }
    }
}
