using Sharpist.Server.Domain.Configurations;
using Sharpist.Server.Service.DTOs.Resumes;

namespace Sharpist.Server.Service.IServices;

public interface IResumeService
{
    Task<bool> RemoveAsync(int id);
    Task<ResumeForResultDto> RetrieveByIdAsync(int id);
    Task<IEnumerable<ResumeForResultDto>> RetrieveByVacancyIdAsync(PaginationParams @params, int id);
    Task<ResumeForResultDto> AddAsync(ResumeForCreationDto dto);
    Task<ResumeForResultDto> RetrieveByEmailAsync(string email);
    Task<IEnumerable<ResumeForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
