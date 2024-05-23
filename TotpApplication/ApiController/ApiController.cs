using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TotpApplication.Service;

namespace TotpApplication.ApiController
{
    //private readonly HttpClient _httpClient;
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        [HttpGet]
        public IActionResult GenerateSecretKey()
        {
            string secretKey = TotpHelper.GenerateSecretKey();
            return Ok(secretKey);
        }

        [HttpGet]
        public IActionResult Validate(string code, string secretKey)
        {
            if (TotpHelper.ValidateTotp(secretKey, code))
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Invalid Code. Please try again");
            }
        }
    }
}
