using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sharpist.Server.Data.IRepositories;
using Sharpist.Server.Domain.Configurations;
using Sharpist.Server.Domain.Entities;
using Sharpist.Server.Service.Commons.Extentions;
using Sharpist.Server.Service.DTOs.HRs;
using Sharpist.Server.Service.Exceptions;
using Sharpist.Server.Service.Helpers;
using Sharpist.Server.Service.IServices;
using System.ComponentModel.DataAnnotations;

namespace Sharpist.Server.Service.Services;

public class HRService : IHRService
{
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;
    private readonly IRepository<HR> repository;

    public HRService(
        IMapper mapper,
        IConfiguration configuration,
        IRepository<HR> repository)
    {
        this.mapper = mapper;
        this.configuration = configuration;
        this.repository = repository;
    }

    public async Task<HRForResultDto> AddAsync(HRForCreationDto dto)
    {
        var user = await this.repository.SelectAsync(u => u.Email == dto.Email);
        if (user is not null)
            throw new CustomException(403, "HR is already exists");

        var hasherResult = PasswordHelper.Hash(dto.Password);
        var mapped = this.mapper.Map<HR>(dto);

        mapped.CreatedAt = DateTime.UtcNow;
        mapped.Password = hasherResult.Hash;

        var result = await this.repository.InsertAsync(mapped);
        await this.repository.SaveAsync();
        return this.mapper.Map<HRForResultDto>(result);
    }

   

    public async Task<bool> ForgetPasswordAsync(string Email, string NewPassword, string ConfirmPassword)
    {
        var user = await this.repository.SelectAsync(u => u.Email == Email);

        if (user is null)
            throw new CustomException(404, "HR not found");

        if (NewPassword != ConfirmPassword)
            throw new CustomException(400, "New password and confirm password aren't equal");

        var hash = PasswordHelper.Hash(NewPassword);

        user.Password = hash.Hash;

        var updated = this.repository.Update(user);

        return await this.repository.SaveAsync();
    }

    public async Task<HRForResultDto> ModifyAsync(int id, HRForUpdateDto dto)
    {
        var user = await this.repository.SelectAsync(u => u.Id == id);
        if (user is null)
            throw new CustomException(404, "HR not found");

        if (dto is not null)
        {
            user.Name = string.IsNullOrEmpty(dto.Name) ? user.Name : dto.Name;
            user.LastName = string.IsNullOrEmpty(dto.LastName) ? user.LastName : dto.LastName;
            user.Email = string.IsNullOrEmpty(dto.Email) ? user.Email: dto.Email;

            user.UpdatedAt = DateTime.UtcNow;
            this.repository.Update(user);
            var result = await this.repository.SaveAsync();
        }
        var person = this.mapper.Map(dto, user);
        /* await this.repository.SaveAsync();*/

        return this.mapper.Map<HRForResultDto>(person);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var user = await repository.SelectAsync(u => u.Id == id);
        if (user is null)
            throw new CustomException(404, "HR not found");

        await repository.DeleteAsync(id);
        var result = await this.repository.SaveAsync();
        return result;
    }

    public async Task<IEnumerable<HRForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var users = await this.repository.SelectAll()
            .Where(x => x.Email != null)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        var mappedUsers = this.mapper.Map<IEnumerable<HRForResultDto>>(users);

        return mappedUsers;
    }

    public async Task<HRForResultDto> RetrieveByIdAsync(int id)
    {
        var user = await this.repository.SelectAll()
        .Where(u => u.Id == id)
        .AsNoTracking()
        .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "HR Not Found");

        var userDto = this.mapper.Map<HRForResultDto>(user);
        return userDto;
    }



    public async Task<HRForResultDto> RetrieveByEmailAsync(string Email)
    {
        var user = await this.repository.SelectAsync(u => u.Email == Email);
        if (user is null)
            throw new CustomException(404, "HR Not Found");

        return this.mapper.Map<HRForResultDto>(user);
    }
}
