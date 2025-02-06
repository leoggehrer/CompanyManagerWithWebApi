namespace CompanyManager.WebApi.Controllers
{
    using TModel = Models.Employee;
    using TEntity = Logic.Entities.Employee;
    using CompanyManager.WebApi.Contracts;

    public class EmployeesController : GenericController<TModel, TEntity>
    {
        public EmployeesController(IContextAccessor contextAccessor) : base(contextAccessor)
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
