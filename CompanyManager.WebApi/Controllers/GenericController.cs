using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Web;

namespace CompanyManager.WebApi.Controllers
{
    /// <summary>
    /// A generic controller for handling CRUD operations.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GenericController<TModel, TEntity> : ControllerBase
        where TModel : Models.ModelObject, new()
        where TEntity : Logic.Entities.EntityObject, new()
    {
        private const int MaxCount = 500;

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <returns>The context.</returns>
        protected abstract Logic.Contracts.IContext GetContext();

        /// <summary>
        /// Gets the DbSet.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The DbSet.</returns>
        protected abstract DbSet<TEntity> GetDbSet(Logic.Contracts.IContext context);

        /// <summary>
        /// Converts an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The model.</returns>
        protected abstract TModel ToModel(TEntity entity);

        /// <summary>
        /// Gets all models.
        /// </summary>
        /// <returns>A list of models.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual ActionResult<IEnumerable<TModel>> Get()
        {
            using var context = GetContext();
            var dbSet = GetDbSet(context);
            var querySet = dbSet.AsQueryable().AsNoTracking();
            var query = querySet.Take(MaxCount).ToArray();
            var result = query.Select(e => ToModel(e));

            return Ok(result);
        }

        /// <summary>
        /// Queries models based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A list of models.</returns>
        [HttpGet("/api/[controller]/query/{predicate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual ActionResult<IEnumerable<TModel>> Query(string predicate)
        {
            using var context = GetContext();
            var dbSet = GetDbSet(context);
            var querySet = dbSet.AsQueryable().AsNoTracking();
            var query = querySet.Where(HttpUtility.UrlDecode(predicate)).Take(MaxCount).ToArray();
            var result = query.Select(e => ToModel(e)).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets a model by ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The model.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual ActionResult<TModel?> Get(int id)
        {
            using var context = GetContext();
            var dbSet = GetDbSet(context);
            var result = dbSet.FirstOrDefault(e => e.Id == id);

            return result == null ? NotFound() : Ok(ToModel(result));
        }

        /// <summary>
        /// Creates a new model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The created model.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual ActionResult<TModel> Post([FromBody] TModel model)
        {
            try
            {
                using var context = GetContext();
                var dbSet = GetDbSet(context);
                var entity = new TEntity();

                entity.CopyProperties(model);
                dbSet.Add(entity);
                context.SaveChanges();

                return CreatedAtAction("Get", new { id = entity.Id }, Ok(ToModel(entity)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates a model by ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="model">The model.</param>
        /// <returns>The updated model.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual ActionResult<TModel> Put(int id, [FromBody] TModel model)
        {
            try
            {
                using var context = GetContext();
                var dbSet = GetDbSet(context);
                var entity = dbSet.FirstOrDefault(e => e.Id == id);

                if (entity != null)
                {
                    entity.CopyProperties(model);
                    context.SaveChanges();
                }
                return entity == null ? NotFound() : Ok(ToModel(entity));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Partially updates a model by ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="patchModel">The patch document.</param>
        /// <returns>The updated model.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual ActionResult<TModel> Patch(int id, [FromBody] JsonPatchDocument<TModel> patchModel)
        {
            try
            {
                using var context = GetContext();
                var dbSet = GetDbSet(context);
                var entity = dbSet.FirstOrDefault(e => e.Id == id);

                if (entity != null)
                {
                    var model = ToModel(entity);

                    patchModel.ApplyTo(model);

                    entity.CopyProperties(model);
                    context.SaveChanges();
                }
                return entity == null ? NotFound() : Ok(ToModel(entity));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a model by ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual ActionResult Delete(int id)
        {
            try
            {
                using var context = GetContext();
                var dbSet = GetDbSet(context);
                var entity = dbSet.FirstOrDefault(e => e.Id == id);

                if (entity != null)
                {
                    dbSet.Remove(entity);
                    context.SaveChanges();
                }
                return entity == null ? NotFound() : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}