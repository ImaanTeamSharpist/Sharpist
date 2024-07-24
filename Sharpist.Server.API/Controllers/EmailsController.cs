using Microsoft.AspNetCore.Mvc;
using Sharpist.Server.API.Controllers.Commons;
using Sharpist.Server.API.Models;
using Sharpist.Server.Service.IServices;


namespace Sharpist.Server.API.Controllers;

public class EmailsController : BaseController
{ 
    private readonly IEmailService emailService;

    public EmailsController(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    /// <summary>
    /// Create new users
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<bool>> PostAsync([FromBody] string to, string subject, string message)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await emailService.SendMessageToEmailAsync(to, subject, message)
        });
}
