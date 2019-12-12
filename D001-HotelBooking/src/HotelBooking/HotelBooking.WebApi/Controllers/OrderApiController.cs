using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Models.Orders;
using HotelBooking.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.WebApi.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderApiController : Controller
    {
        private readonly IOrderService orderService;

        public OrderApiController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [Route("of-client/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByClient(int id) => Json(await orderService.GetByClientAsync(id));

        [Route("of-hotel/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByHotel(int id) => Json(await orderService.GetByHotelAsync(id));

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> Add(AddOrderModel addOrderModel)
            => Json(await orderService.AddAsync(addOrderModel));
    }
}