using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sharpist.Server.API.Controllers.Commons;
using Sharpist.Server.API.Models;
using Sharpist.Server.Domain.Configurations;
using Sharpist.Server.Service.DTOs.HRs;
using Sharpist.Server.Service.IServices;
using System.ComponentModel.DataAnnotations;

namespace Sharpist.Server.API.Controllers;

public class HRsController : BaseController
{
    private readonly IHRService hrService;
    public HRsController(IHRService userService)
    {
        this.hrService = userService;
    }

    /// <summary>
    /// Create new users
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<HRForResultDto>> PostAsync([FromBody] HRForCreationDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await hrService.AddAsync(dto)
        });

    /// <summary>
    /// Get all users
    /// </summary>
    /// <param name="params"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await hrService.RetrieveAllAsync(@params)
        });

    /// <summary>
    /// Get by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] int id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await hrService.RetrieveByIdAsync(id)
        });


    /// <summary>
    /// Update users info
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    //[Authorize(Roles = "Admin", "User")]
    [HttpPut("{id}")]
    public async Task<ActionResult<HRForResultDto>> PutAsync([FromRoute(Name = "id")] int id, [FromBody] HRForUpdateDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await hrService.ModifyAsync(id, dto)
        });

    /// <summary>
    /// Delete by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync([FromRoute(Name = "id")] int id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await hrService.RemoveAsync(id)
        });


    [AllowAnonymous]
    [HttpPut("forget-password")]
    public async Task<IActionResult> ForgetPasswordAsync([Required] string Email, [Required] string NewPassword, [Required] string ConfirmPassword)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await hrService.ForgetPasswordAsync(Email, NewPassword, ConfirmPassword)
        });


    [AllowAnonymous]
    [HttpGet("email")]
    public async Task<IActionResult> RetrievePhoneNumberAsync(string Email)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.hrService.RetrieveByEmailAsync(Email)
        });
}
