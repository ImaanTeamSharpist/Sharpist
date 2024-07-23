using Sharpist.Server.Domain.Entities;

namespace Sharpist.Server.Service.IServices;

public interface IAuthService
{
    public string GenerateToken(HR users);
}
