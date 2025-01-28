using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CompanyManager.WebApi.Controllers
{
    using TModel = Models.Company;


    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        public const int MaxCount = 500;

        // GET: api/<CompaniesController>
        [HttpGet]
        public IEnumerable<TModel> Get()
        {
            using var context = Logic.DataContext.Factory.CreateContext();

            return context.CompanySet.Take(MaxCount).AsNoTracking().Select(e => TModel.Create(e)).ToArray();
        }

        // GET api/<CompaniesController>/5
        [HttpGet("{id}")]
        public TModel? Get(int id)
        {
            using var context = Logic.DataContext.Factory.CreateContext();
            var result = context.CompanySet.FirstOrDefault(e => e.Id == id);

            return result != null ? TModel.Create(result) : null;
        }

        // POST api/<CompaniesController>
        [HttpPost]
        public void Post([FromBody] TModel model)
        {
            using var context = Logic.DataContext.Factory.CreateContext();
            var entity = new Logic.Entities.Company();

            entity.CopyProperties(model);
            context.CompanySet.Add(entity);
            context.SaveChanges();
        }

        // PUT api/<CompaniesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] TModel model)
        {
            using var context = Logic.DataContext.Factory.CreateContext();
            var result = context.CompanySet.FirstOrDefault(e => e.Id == id);

            if (result != null)
            {
                result.CopyProperties(model);
                context.SaveChanges();
            }
        }

        // DELETE api/<CompaniesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using var context = Logic.DataContext.Factory.CreateContext();
            var result = context.CompanySet.FirstOrDefault(e => e.Id == id);

            if (result != null)
            {
                context.CompanySet.Remove(result);
                context.SaveChanges();
            }
        }
    }
}
