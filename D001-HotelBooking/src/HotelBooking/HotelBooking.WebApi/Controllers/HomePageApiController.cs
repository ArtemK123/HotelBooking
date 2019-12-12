using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.WebApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomePageApiController : Controller
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Server is working. All right - just relax and start testing ;-)";
        }
    }
}