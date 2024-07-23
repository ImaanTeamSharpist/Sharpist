namespace Sharpist.Server.Service.DTOs.Vacancies;

public class VacancyForUpdateDto
{
    public string JobName { get; set; } = "";
    public string JobDescription { get; set; } = "";
    public string Requirements { get; set; } = "";
    public string Salary { get; set; } = "";
    public bool IsActive { get; set; } = true;
}
