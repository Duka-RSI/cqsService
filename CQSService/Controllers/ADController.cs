using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CQSService.Models;

namespace CQSService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ADController : ControllerBase
    {

        private readonly QuotaionContext _context;
        public ADController(QuotaionContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserAD()
        {
            var user = new IdentityUser
            {
                Username = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                AuthenticationType = User.Identity.AuthenticationType
            };

            if (User.Identity.IsAuthenticated) {

                SysUser ADuser = _context.SysUsers.Where(x=>x.Adusername == User.Identity.Name).FirstOrDefault();
                if (ADuser == null)
                {
                    return NotFound();
                }


                return Ok(new { name = ADuser.Username, password = common.commonFunction.Base64Decode(ADuser.Password) });
            }
            return NotFound();
        }

        public class IdentityUser
        {
            public string Username { get; set; }
            public bool IsAuthenticated { get; set; }
            public string AuthenticationType { get; set; }
        }


    }
}
