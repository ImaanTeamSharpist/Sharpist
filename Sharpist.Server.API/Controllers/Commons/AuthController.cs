using Microsoft.AspNetCore.Mvc;
using Sharpist.Server.API.Models;
using Sharpist.Server.Service.DTOs.Login;
using Sharpist.Server.Service.IServices;

namespace Sharpist.Server.API.Controllers.Commons;

public class AuthController : BaseController
{
    private readonly IAccountService accountService;

    public AuthController(IAccountService accountService, IAuthService authService)
    {
        this.accountService = accountService;
    }

    [HttpPost]
    [Route("login")]
    public async ValueTask<IActionResult> login([FromBody] LoginDto loginDto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await accountService.LoginAsync(loginDto)
        });
}
