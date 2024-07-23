using Sharpist.Server.Domain.Entities.Commons;

namespace Sharpist.Server.Domain.Entities;

public class HR : Auditable
{
    public string Name { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}
