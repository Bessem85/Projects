using System;
using System.Threading.Tasks;
using System.Web.Http;

using ExoWebApi.DAL.Contracts;
using ExoWebApi.DAL.Repositories;
using ExoWebApi.Web.Models.User;
using VPwebapitest.Filters;

namespace ExoWebApi.Web.Controllers
{
    [RoutePrefix("api/v1/User")]
    public class UserController : ApiController
    {
        private IUserRepository _usersrepository;
        public UserController()
        {
            _usersrepository = new UserRepository();
        }

        [Route("Authenticate")]
        [HttpPost]
        public async Task<IHttpActionResult> Authenticate([FromBody]AutentificationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Email et mot de passe sont obligatoires");
            try
            {
                var result = await _usersrepository.Authentificate(model.Email, model.Password);
                return Ok(result);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("Confidentials")]
        [AmazonAuthentication]
        [HttpPost]
        public async Task<IHttpActionResult> Confidentials([FromBody]AutentificationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Email est obligatoire.");
            try
            {
                var result = await _usersrepository.Confidentials(model.Email);
                return Ok(result);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
