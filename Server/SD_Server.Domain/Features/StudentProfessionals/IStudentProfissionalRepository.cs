using SD_Server.Domain.Base;
using SD_Server.Domain.Features.Students;
using SD_SharedKernel.Helpers;

namespace SD_Server.Domain.Features.StudentProfessionals;

public interface IStudentProfissionalRepository : IRepositoryBase<StudentProfessional>
{
    public Task <Result<Exception,IQueryable<Student>>> GetAllUserIdByProfessionalId(int professionalId);
}