using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sharpist.Server.API.Controllers.Commons;
using Sharpist.Server.API.Models;
using Sharpist.Server.Domain.Configurations;
using Sharpist.Server.Service.DTOs.Vacancies;
using Sharpist.Server.Service.IServices;
using System.ComponentModel.DataAnnotations;

namespace Sharpist.Server.API.Controllers;

public class VacanciesController : BaseController
{
    private readonly IVacancyService vacancyService;
    public VacanciesController(IVacancyService userService)
    {
        this.vacancyService = userService;
    }

    /// <summary>
    /// Create new users
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<VacancyForResultDto>> PostAsync([FromBody] VacancyForCreationDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await vacancyService.AddAsync(dto)
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
            Data = await vacancyService.RetrieveAllAsync(@params)
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
            Data = await vacancyService.RetrieveByIdAsync(id)
        });


    /// <summary>
    /// Update users info
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    //[Authorize(Roles = "Admin", "User")]
    [HttpPut("{id}")]
    public async Task<ActionResult<VacancyForResultDto>> PutAsync([FromRoute(Name = "id")] int id, [FromBody] VacancyForUpdateDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await vacancyService.ModifyAsync(id, dto)
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
            Data = await vacancyService.RemoveAsync(id)
        });


    [AllowAnonymous]
    [HttpGet("jobname")]
    public async Task<IActionResult> RetrieveByJobNameAsync(string JobName)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.vacancyService.RetrieveByJobNameAsync(JobName)
        });

    [AllowAnonymous]
    [HttpGet("salary")]
    public async Task<IActionResult> RetrieveBySalaryAsync(string Salary)
    => Ok(new Response
    {
        Code = 200,
        Message = "Success",
        Data = await this.vacancyService.RetrieveBySalaryAsync(Salary)
    });
}
