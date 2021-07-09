using Microsoft.AspNetCore.Mvc;
using NetCore.Infrastructure.Database.Contexts;
using System.Linq;

namespace NetCore.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        public ActivityController(DatabaseContext context)
        {
            _databaseContext = context;
        }

        // GET api/values
        [HttpGet]
        //[Authorize(Roles="user")]
        //[Authorize]
        public ActionResult<string> Get()
        {
            var result = _databaseContext.Activity.Take(10); 
            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
