using Sharpist.Server.Domain.Configurations;
using Sharpist.Server.Service.DTOs.Vacancies;

namespace Sharpist.Server.Service.IServices;

public interface IVacancyService
{
    Task<bool> RemoveAsync(int id);
    Task<VacancyForResultDto> RetrieveByIdAsync(int id);
    Task<VacancyForResultDto> AddAsync(VacancyForCreationDto dto);
    Task<VacancyForResultDto> ModifyAsync(int id, VacancyForUpdateDto dto);
    Task<VacancyForResultDto> RetrieveByJobNameAsync(string JobName);
    Task<VacancyForResultDto> RetrieveBySalaryAsync(string Salary);
    Task<IEnumerable<VacancyForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
