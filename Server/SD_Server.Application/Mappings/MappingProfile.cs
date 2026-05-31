using AutoMapper;
using SD_Server.Application.Features.Professionals.DTO;
using SD_Server.Application.Features.Students.DTO;
using SD_Server.Domain.Features.Professionals;
using SD_Server.Domain.Features.Students;

namespace SD_Server.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Professional, ProfessionalDTO>();
            CreateMap<Student, StudentDTO>();
        }
    }
}
