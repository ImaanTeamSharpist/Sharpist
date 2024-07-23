using AutoMapper;
using Sharpist.Server.Domain.Entities;
using Sharpist.Server.Service.DTOs.HRs;
using Sharpist.Server.Service.DTOs.Resumes;
using Sharpist.Server.Service.DTOs.Vacancies;

namespace Sharpist.Server.Service.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // HR
        CreateMap<HR, HRForCreationDto>().ReverseMap();
        CreateMap<HR, HRForResultDto>().ReverseMap();
        CreateMap<HR, HRForUpdateDto>().ReverseMap();

        // Resume
        CreateMap<Resume, ResumeForCreationDto>().ReverseMap();
        CreateMap<Resume, ResumeForResultDto>().ReverseMap();

        // Vacancy
        CreateMap<Vacancy, VacancyForCreationDto>().ReverseMap();
        CreateMap<Vacancy, VacancyForResultDto>().ReverseMap();
        CreateMap<Vacancy, VacancyForUpdateDto>().ReverseMap();
    }
}
