using Microsoft.AspNetCore.Mvc;

namespace NetCore.Api.Controllers
{

#if RELEASE
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "user")]
#endif
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizedBaseController : ControllerBase
    {
    }
}
