using CompanyManager.Logic.Contracts;
using CompanyManager.WebApi.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.WebApi.Controllers
{
    using TModel = Models.Customer;
    using TEntity = Logic.Entities.Customer;

    public class CustomersController : GenericController<TModel, TEntity>
    {
        public CustomersController(IContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override TModel ToModel(TEntity entity)
        {
            var result = new TModel();

            result.CopyProperties(entity);
            return result;
        }
    }
}
