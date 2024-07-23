using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sharpist.Server.Data.IRepositories;
using Sharpist.Server.Domain.Configurations;
using Sharpist.Server.Domain.Entities;
using Sharpist.Server.Service.Commons.Extentions;
using Sharpist.Server.Service.DTOs.Vacancies;
using Sharpist.Server.Service.Exceptions;
using Sharpist.Server.Service.Helpers;
using Sharpist.Server.Service.IServices;

namespace Sharpist.Server.Service.Services;

public class VacancyService : IVacancyService
{
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;
    private readonly IRepository<Vacancy> repository;

    public VacancyService(
        IMapper mapper,
        IConfiguration configuration,
        IRepository<Vacancy> repository)
    {
        this.mapper = mapper;
        this.configuration = configuration;
        this.repository = repository;
    }

    public async Task<VacancyForResultDto> AddAsync(VacancyForCreationDto dto)
    {
        var user = await this.repository.SelectAsync(u => u.JobName == dto.JobName && u.Requirements == dto.Requirements);
        if (user.IsActive)
            throw new CustomException(403, "Vacancy is already exists");

        var mapped = this.mapper.Map<Vacancy>(dto);

        mapped.CreatedAt = DateTime.UtcNow;

        var result = await this.repository.InsertAsync(mapped);
        await this.repository.SaveAsync();
        return this.mapper.Map<VacancyForResultDto>(result);
    }


    public async Task<VacancyForResultDto> ModifyAsync(int id, VacancyForUpdateDto dto)
    {
        var user = await this.repository.SelectAsync(u => u.Id == id);
        if (!user.IsActive)
            throw new CustomException(404, "Vacancy not found");

        if (dto is not null)
        {
            user.Requirements = string.IsNullOrEmpty(dto.Requirements.ToLower()) ? user.Requirements.ToLower() : dto.Requirements.ToLower();
            user.JobName = string.IsNullOrEmpty(dto.JobName.ToLower()) ? user.JobName.ToLower() : dto.JobName.ToLower();
            user.Salary = string.IsNullOrEmpty(dto.Salary.ToLower()) ? user.Salary.ToLower() : dto.Salary.ToLower();
            user.JobDescription = string.IsNullOrEmpty(dto.JobDescription.ToLower()) ? user.JobDescription.ToLower() : dto.JobDescription.ToLower();

            user.UpdatedAt = DateTime.UtcNow;
            this.repository.Update(user);
            var result = await this.repository.SaveAsync();
        }
        var person = this.mapper.Map(dto, user);
        /* await this.repository.SaveAsync();*/

        return this.mapper.Map<VacancyForResultDto>(person);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var user = await repository.SelectAsync(u => u.Id == id);
        if (!user.IsActive)
            throw new CustomException(404, "Vacancy not found");

        user.IsActive = false;
        var result = await this.repository.SaveAsync();
        return result;
    }

    public async Task<IEnumerable<VacancyForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var users = await this.repository.SelectAll()
            .Where(x => x.IsActive)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        var mappedUsers = this.mapper.Map<IEnumerable<VacancyForResultDto>>(users);

        return mappedUsers;
    }

    public async Task<VacancyForResultDto> RetrieveByIdAsync(int id)
    {
        var user = await this.repository.SelectAll()
        .Where(u => u.Id == id && u.IsActive)
        .AsNoTracking()
        .FirstOrDefaultAsync();

        if (!user.IsActive)
            throw new CustomException(404, "Vacancy Not Found");

        var userDto = this.mapper.Map<VacancyForResultDto>(user);
        return userDto;
    }

    public async Task<VacancyForResultDto> RetrieveByJobNameAsync(string JobName)
    {
        var user = await this.repository.SelectAll()
        .Where(u => u.JobName.ToLower() == JobName.ToLower() && u.IsActive)
        .AsNoTracking()
        .FirstOrDefaultAsync();

        if (!user.IsActive)
            throw new CustomException(404, "Vacancy Not Found");

        var userDto = this.mapper.Map<VacancyForResultDto>(user);
        return userDto;
    }

    public async Task<VacancyForResultDto> RetrieveBySalaryAsync(string Salary)
    {
        var user = await this.repository.SelectAll()
        .Where(u => u.Salary.ToLower()== Salary.ToLower() && u.IsActive)
        .AsNoTracking()
        .FirstOrDefaultAsync();

        if (!user.IsActive)
            throw new CustomException(404, "Vacancy Not Found");

        var userDto = this.mapper.Map<VacancyForResultDto>(user);
        return userDto;
    }
}
