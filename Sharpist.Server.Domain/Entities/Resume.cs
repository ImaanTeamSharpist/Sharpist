using Sharpist.Server.Domain.Entities.Commons;

namespace Sharpist.Server.Domain.Entities;

public class Resume : Auditable
{
    public int VacancyID { get; set; }
    public string FilePath { get; set; } = "";
    public string Email { get; set; } = "";
    public string Text { get; set; } = "";
}
