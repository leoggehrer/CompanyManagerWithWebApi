using CompanyManager.Logic.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.WebApi.Controllers
{
    using TModel = Models.Customer;
    using TEntity = Logic.Entities.Customer;

    public class CustomersController : GenericController<TModel, TEntity>
    {
        protected override IContext GetContext()
        {
            return Logic.DataContext.Factory.CreateContext();
        }

        protected override DbSet<TEntity> GetDbSet(IContext context)
        {
            return context.CustomerSet;
        }

        protected override TModel ToModel(TEntity entity)
        {
            var result = new TModel();

            result.CopyProperties(entity);
            return result;
        }
    }
}
