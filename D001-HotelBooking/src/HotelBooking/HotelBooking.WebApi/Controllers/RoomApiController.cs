using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Models.Search;
using HotelBooking.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.WebApi.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomApiController : Controller
    {
        private readonly IRoomService roomService;

        public RoomApiController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get() => Json(await roomService.GetAsync());

        [HttpGet]
        public async Task<IActionResult> Get(int id) => Json(await roomService.GetAsync(id));

        [HttpPut]
        [Route("search")]
        public IActionResult Search(RoomRequirements roomRequirements) =>
            Json(roomService.SearchByFilters(roomRequirements));

        [HttpGet]
        [Route("getbyhotelid/{id}")]
        public async Task<IActionResult> GetByHotelId(int id) => Json(await roomService.GetByHotelIdAsync(id));
    }
}