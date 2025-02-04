using CompanyManager.Logic.Contracts;
using CompanyManager.Logic.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.WebApi.Contracts
{
    public interface IContextAccessor<TEntity> where TEntity : EntityObject, new()
    {
        IContext GetContext();
        DbSet<TEntity>? GetDbSet();
    }
}