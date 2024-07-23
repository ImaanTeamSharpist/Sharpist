using Sharpist.Server.Service.DTOs.Login;

namespace Sharpist.Server.Service.IServices;

public interface IAccountService
{
    public Task<string> LoginAsync(LoginDto loginDto);
}