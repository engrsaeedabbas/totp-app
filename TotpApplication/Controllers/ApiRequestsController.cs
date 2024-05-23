using Microsoft.AspNetCore.Mvc;
using TotpApplication.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TotpApplication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiRequestsController : ControllerBase
    {
        // GET: api/<ApiRequestsController>
        [HttpGet]
        public IActionResult GetSecretKey()
        {
            string secretKey = TotpHelper.GenerateSecretKey();
            
            string result = "Your secret key is: " + secretKey;
            return Ok(result);
        }

        // GET api/<ApiRequestsController>
        [HttpGet]
        public IActionResult Validate(string code, string secretKey)
        {
            if(TotpHelper.ValidateTotp(secretKey, code))
            {
                return Ok(true);
            }else
            {
                return BadRequest("Invalid Code. Please try again");
            }
        }
    }
}
