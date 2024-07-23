using Sharpist.Server.Domain.Configurations;
using Sharpist.Server.Service.DTOs.HRs;

namespace Sharpist.Server.Service.IServices;

public interface IHRService 
{
    Task<bool> RemoveAsync(int id);
    Task<HRForResultDto> RetrieveByIdAsync(int id);
    Task<HRForResultDto> AddAsync(HRForCreationDto dto);
    Task<HRForResultDto> ModifyAsync(int id, HRForUpdateDto dto);
    Task<HRForResultDto> RetrieveByEmailAsync(string Email);
    Task<IEnumerable<HRForResultDto>> RetrieveAllAsync(PaginationParams @params);
    Task<bool> ForgetPasswordAsync(string Email, string NewPassword, string ConfirmPassword);
}
