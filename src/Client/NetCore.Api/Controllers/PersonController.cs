using Microsoft.AspNetCore.Mvc;
using NetCore.Infrastructure.Database.Contexts;
using NetCore.Infrastructure.Database.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Api.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public PersonController(DatabaseContext context)
        {
            _databaseContext = context;
        }

        // GET api/values
        [HttpGet]
        //[Authorize(Roles="user")]
        //[Authorize]
        public ActionResult<string> Get()
        {
            var result = _databaseContext.Set<Person>().Take(10);
            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Person model)
        {
            var result = await _databaseContext.Set<Person>().AddAsync(model);
            return Ok(result);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existing = _databaseContext.Set<Person>().FirstOrDefault(x => x.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            return Accepted();
        }
    }
}
