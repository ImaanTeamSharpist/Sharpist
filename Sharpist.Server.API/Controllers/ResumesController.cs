using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sharpist.Server.API.Controllers.Commons;
using Sharpist.Server.API.Models;
using Sharpist.Server.Domain.Configurations;
using Sharpist.Server.Service.DTOs.Resumes;
using Sharpist.Server.Service.IServices;
namespace Sharpist.Server.API.Controllers;

public class ResumesController : BaseController
{
    private readonly IResumeService resumeService;
    public ResumesController(IResumeService rService)
    {
        this.resumeService = rService;
    }

    /// <summary>
    /// Create new users
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ResumeForResultDto>> PostAsync([FromBody] ResumeForCreationDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await resumeService.AddAsync(dto)
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
            Data = await resumeService.RetrieveAllAsync(@params)
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
            Data = await resumeService.RetrieveByIdAsync(id)
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
            Data = await resumeService.RemoveAsync(id)
        });


    [AllowAnonymous]
    [HttpGet("email")]
    public async Task<IActionResult> RetrieveByJobNameAsync(string Email)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.resumeService.RetrieveByEmailAsync(Email)
        });

    [AllowAnonymous]
    [HttpGet("vacancy-id")]
    public async Task<IActionResult> RetrieveBySalaryAsync(int VacancyId, [FromQuery] PaginationParams @params)
    => Ok(new Response
    {
        Code = 200,
        Message = "Success",
        Data = await this.resumeService.RetrieveByVacancyIdAsync(@params, VacancyId)
    });
}
