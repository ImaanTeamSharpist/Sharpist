using Sharpist.Server.Data.IRepositories;
using Sharpist.Server.Domain.Entities;
using Sharpist.Server.Service.DTOs.Login;
using Sharpist.Server.Service.Exceptions;
using Sharpist.Server.Service.Helpers;
using Sharpist.Server.Service.IServices;

namespace Sharpist.Server.Service.Services;

public class AccountService : IAccountService
{
    private readonly IAuthService authService;
    private readonly IRepository<HR> userRepository;

    public AccountService(IRepository<HR> userRepository, IAuthService authService)
    {
        this.authService = authService;
        this.userRepository = userRepository;
    }
    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await userRepository.SelectAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());
        if (user is null)
            throw new CustomException(404, "Email or password entered wrong!");

        var hasherResult = PasswordHelper.Verify(loginDto.Password, user.Password);
        if (hasherResult == false)
            throw new CustomException(404, "Email or password entered wrong!");

        return authService.GenerateToken(user);
    }
}
