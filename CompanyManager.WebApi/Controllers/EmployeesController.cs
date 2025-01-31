namespace CompanyManager.WebApi.Controllers
{
    using TModel = Models.Employee;
    using TEntity = Logic.Entities.Employee;

    public class EmployeesController : GenericController<TModel, TEntity>
    {
        protected override Logic.Contracts.IContext GetContext()
        {
            return Logic.DataContext.Factory.CreateContext();
        }
        protected override Microsoft.EntityFrameworkCore.DbSet<TEntity> GetDbSet(Logic.Contracts.IContext context)
        {
            return context.EmployeeSet;
        }
        protected override TModel ToModel(TEntity entity)
        {
            var result = new TModel();
            result.CopyProperties(entity);
            return result;
        }
    }
}
