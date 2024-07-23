using Microsoft.AspNetCore.Http;

namespace Sharpist.Server.Service.DTOs.Resumes;

public class ResumeForCreationDto
{
    public int VacancyID { get; set; }
    public IFormFile FilePath { get; set; } 
    public string Email { get; set; } = "";
    public string Text { get; set; } = "";
}
