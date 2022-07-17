using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace API.Controllers
{
    [Route("IdentityServer")]
    [Authorize("ApiScope")]
    public class IdentityServerController : ControllerBase
    {
       
       public IActionResult Get()
        {
            return new JsonResult(from claim in User.Claims select new { claim.Type,claim.Value });
        }
    }
}
