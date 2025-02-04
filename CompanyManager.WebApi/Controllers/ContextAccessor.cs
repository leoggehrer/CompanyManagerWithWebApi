﻿using Microsoft.EntityFrameworkCore;

namespace CompanyManager.WebApi.Controllers
{
    public class ContextAccessor<TModel, TEntity> : IDisposable
        where TModel : Models.ModelObject, new()
        where TEntity : Logic.Entities.EntityObject, new()

    {
        #region fields
        Logic.Contracts.IContext? context = null;

        #endregion fields

        public Logic.Contracts.IContext GetContext() => context ??= Logic.DataContext.Factory.CreateContext();

        public DbSet<TEntity>? GetDbSet()
        {
            DbSet<TEntity>? result = default;

            if (typeof(TEntity) == typeof(Logic.Entities.Company))
            {
                result = GetContext().CompanySet as DbSet<TEntity>;
            }
            else if (typeof(TEntity) == typeof(Logic.Entities.Customer))
            {
                result = GetContext().CustomerSet as DbSet<TEntity>;
            }
            if (typeof(TEntity) == typeof(Logic.Entities.Employee))
            {
                result = GetContext().EmployeeSet as DbSet<TEntity>;
            }

            return result;
        }

        public void Dispose()
        {
            context?.Dispose();
            context = null;
        }
    }
}
