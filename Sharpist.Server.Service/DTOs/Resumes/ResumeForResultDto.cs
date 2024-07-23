namespace Sharpist.Server.Service.DTOs.Resumes;

public class ResumeForResultDto
{
    public int VacancyID { get; set; }
    public string FilePath { get; set; } = "";
    public string Email { get; set; } = "";
    public string Text { get; set; } = "";
}
