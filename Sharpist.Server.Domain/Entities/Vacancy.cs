using Sharpist.Server.Domain.Entities.Commons;

namespace Sharpist.Server.Domain.Entities;

public class Vacancy : Auditable
{
    public string JobName { get; set; } = "";
    public string JobDescription { get; set; } = "";
    public string Requirements { get; set; } = "";
    public string Salary { get; set; } = "";
    public bool IsActive { get; set; } = true;
}
