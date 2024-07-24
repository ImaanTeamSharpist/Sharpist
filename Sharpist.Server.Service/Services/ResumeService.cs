using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sharpist.Server.Data.IRepositories;
using Sharpist.Server.Domain.Configurations;
using Sharpist.Server.Domain.Entities;
using Sharpist.Server.Service.Commons.Extentions;
using Sharpist.Server.Service.DTOs.Resumes;
using Sharpist.Server.Service.Exceptions;
using Sharpist.Server.Service.Helpers;
using Sharpist.Server.Service.IServices;
using UglyToad.PdfPig;

namespace Sharpist.Server.Service.Services;

public class ResumeService : IResumeService
{
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;
    private readonly IRepository<Resume> repository;
    private readonly IRepository<Vacancy> vacancyRepository;
    private readonly IPdfToTextService pdfToTextService;
    private readonly IOpenAIHelperService openAIHelperService;
    private readonly IEmailService emailService;
    public ResumeService(
        IMapper mapper,
        IConfiguration configuration,
        IRepository<Resume> repository,
        IRepository<Vacancy> vacancyRepository,
        IPdfToTextService pdfToTextService,
        IOpenAIHelperService openAIHelperService,
        IEmailService emailService)
    {
        this.mapper = mapper;
        this.configuration = configuration;
        this.repository = repository;
        this.vacancyRepository = vacancyRepository;
        this.pdfToTextService = pdfToTextService;
        this.openAIHelperService = openAIHelperService;
        this.emailService = emailService;
    }

    public async Task<ResumeForResultDto> AddAsync(ResumeForCreationDto dto)
    {
        var vacancy = await this.vacancyRepository.SelectAsync(v => v.Id == dto.VacancyID && v.IsActive);
        if (vacancy is null)
        {
            throw new CustomException(404, "Vacancy not found");
        }
        var user = await this.repository.SelectAsync(u => u.Email == dto.Email && u.VacancyID == dto.VacancyID);
        if (user is not null)
            throw new CustomException(403, "Resume is already exists");

        var WwwRootPath = Path.Combine(WebHostEnviromentHelper.WebRootPath, "Media", "Resumes");
        var assetsFolderPath = Path.Combine(WwwRootPath, "Media");
        var ImagesFolderPath = Path.Combine(assetsFolderPath, "Resumes");

        if (!Directory.Exists(assetsFolderPath))
        {
            Directory.CreateDirectory(assetsFolderPath);
        }

        if (!Directory.Exists(ImagesFolderPath))
        {
            Directory.CreateDirectory(ImagesFolderPath);
        }
        var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(dto.FilePath.FileName);

        var fullPath = Path.Combine(WwwRootPath, fileName);

        using (var stream = File.OpenWrite(fullPath))
        {
            await dto.FilePath.CopyToAsync(stream);
            await stream.FlushAsync();
            stream.Close();
        }

        string resultImage = Path.Combine("Media", "Resumes", fileName);

        var mapped = mapper.Map<Resume>(dto);
        await this.repository.SaveAsync();
        mapped.FilePath = resultImage;
        mapped.CreatedAt = DateTime.UtcNow;

        //iform file to pdf
        if (dto.FilePath == null || dto.FilePath.Length == 0)
        {
            throw new ArgumentException("Invalid file");
        }

        // Read the file stream
        using (var stream = new MemoryStream())
        {
            dto.FilePath.CopyTo(stream);
            stream.Position = 0; // Reset stream position to the beginning

            // Load the PDF document from the stream
            var pdfDocument = PdfDocument.Open(stream);
            mapped.Text = await pdfToTextService.PdfToTextAsync(pdfDocument);
            string requirements = $"JobName: {vacancy.JobName}, Job Description: {vacancy.JobDescription}, Salary: {vacancy.Salary}, Requirements: {vacancy.Requirements}";

            var openAi = openAIHelperService.CheckResumeCompatibility(mapped.Text, requirements);
            if (openAi != null)
                await emailService.SendMessageToEmailAsync(mapped.Email, "Status of your CV", "Hello! We are checking for your CV now");
        }

        var result = await this.repository.InsertAsync(mapped);
        await this.repository.SaveAsync();

        return this.mapper.Map<ResumeForResultDto>(result);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var user = await repository.SelectAsync(u => u.Id == id);
        if (user is null)
            throw new CustomException(404, "Resume is not found");

        await repository.DeleteAsync(id);
        var result = await this.repository.SaveAsync();
        return result;
    }

    public async Task<IEnumerable<ResumeForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var users = await this.repository.SelectAll()
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        var mappedUsers = this.mapper.Map<IEnumerable<ResumeForResultDto>>(users);

        return mappedUsers;
    }

    public async Task<ResumeForResultDto> RetrieveByIdAsync(int id)
    {
        var user = await this.repository.SelectAll()
        .Where(u => u.Id == id)
        .AsNoTracking()
        .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "Resume Not Found");

        var userDto = this.mapper.Map<ResumeForResultDto>(user);
        return userDto;
    }

    public async Task<IEnumerable<ResumeForResultDto>> RetrieveByVacancyIdAsync(PaginationParams @params, int id)
    {
        var users = await this.repository.SelectAll()
            .Where(u => u.VacancyID == id)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        var mappedUsers = this.mapper.Map<IEnumerable<ResumeForResultDto>>(users);

        return mappedUsers;
    }

    public async Task<ResumeForResultDto> RetrieveByEmailAsync(string Email)
    {
        var user = await this.repository.SelectAll()
        .Where(u => u.Email == Email)
        .AsNoTracking()
        .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "Resume Not Found");

        var userDto = this.mapper.Map<ResumeForResultDto>(user);
        return userDto;
    }
}
