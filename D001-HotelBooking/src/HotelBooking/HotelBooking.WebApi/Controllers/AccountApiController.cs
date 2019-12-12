using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Account;
using HotelBooking.Contracts.Dto.Models.LogIn;
using HotelBooking.Contracts.Dto.Models.Register;
using HotelBooking.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Services.Controllers
{
    [Route("/api/account")]
    [ApiController]
    public class AccountApiController : Controller
    {
        private readonly IAccountService accountService;

        public AccountApiController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestModel userData)
            => Json(await accountService.RegisterAsync(userData));

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LogInRequestModel logInRequestModel)
            => Json(await accountService.LogInAsync(logInRequestModel));

        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> Logout(SessionData sessionData)
        {
            await accountService.LogoutAsync(sessionData);
            return Ok();
        }

        [Route("card/add/{token}")]
        [HttpPost]
        public async Task<IActionResult> AddCreditCard(string token, [FromBody] CreditCardData creditCardData)
            => Json(await accountService.AddCreditCardAsync(token, creditCardData));

        [Route("card/remove/{token}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveCreditCard(string token, [FromBody] CreditCardData creditCardData)
            => Json(await accountService.RemoveCreditCardAsync(token, creditCardData));

        [Route("get/{token}")]
        [HttpGet]
        public async Task<IActionResult> GetClientAccount(string token)
            => Json(await accountService.GetClientAccountAsync(token));

        [Route("update/{token}")]
        [HttpPut]
        public async Task<IActionResult> UpdateClientAccount(string token, [FromBody] UpdateClientRequestModel dto)
            => Json(await accountService.UpdateClientAccountAsync(token, dto));
    }
}