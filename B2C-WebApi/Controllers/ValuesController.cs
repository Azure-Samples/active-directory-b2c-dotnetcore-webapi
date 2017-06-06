using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B2CWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            var scopes = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/scope")?.Value;
            if (!string.IsNullOrEmpty(Startup.ScopeRead) && scopes != null
                    && scopes.Split(' ').Any(s => s.Equals(Startup.ScopeRead)))
                return Ok(new string[] { "value1", "value2" });
            else
                return Unauthorized();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var scopes = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/scope")?.Value;
            if (!string.IsNullOrEmpty(Startup.ScopeRead) && scopes != null
                    && scopes.Split(' ').Any(s => s.Equals(Startup.ScopeRead)))
                return Ok("value1");
            else
                return Unauthorized();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            var scopes = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/scope")?.Value;
            if (!string.IsNullOrEmpty(Startup.ScopeWrite) && scopes != null
                    && scopes.Split(' ').Any(s => s.Equals(Startup.ScopeWrite)))
                // TODO: Post
                return Ok();
            else
                return Unauthorized();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            var scopes = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/scope")?.Value;
            if (!string.IsNullOrEmpty(Startup.ScopeWrite) && scopes != null
                    && scopes.Split(' ').Any(s => s.Equals(Startup.ScopeWrite)))
                // TODO: Put
                return Ok();
            else
                return Unauthorized();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var scopes = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/scope")?.Value;
            if (!string.IsNullOrEmpty(Startup.ScopeWrite) && scopes != null
                    && scopes.Split(' ').Any(s => s.Equals(Startup.ScopeWrite)))
                // TODO: Delete
                return Ok();
            else
                return Unauthorized();
        }
    }
}
