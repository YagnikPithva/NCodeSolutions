using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{

    [ApiController]
    [AllowAnonymous]
    [Route("api/[Controller]")]
    public class HealthController
    {

        [HttpGet]
        public ActionResult Health()
        {
            return new OkObjectResult(new
            {
                message = "Employee API is up and running...",
                timestamp = DateTime.UtcNow.ToString("o"),
                version = typeof(HealthController).Assembly.GetName().Version?.ToString() ?? "Unknown"
            });
        }

   

    }
}
