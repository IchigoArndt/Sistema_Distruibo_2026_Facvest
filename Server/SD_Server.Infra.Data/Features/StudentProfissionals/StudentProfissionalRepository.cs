using Microsoft.EntityFrameworkCore;
using SD_Server.Domain.Features.StudentProfessionals;
using SD_Server.Domain.Features.Students;
using SD_Server.Infra.Data.Context;
using SD_SharedKernel.Helpers;

namespace SD_Server.Infra.Data.Features.StudentProfissionals;

public class StudentProfissionalRepository(SdServerDbContext context) : IStudentProfissionalRepository
{
    public async Task<Result<Exception, int>> AddAsync(StudentProfessional entity)
    {
        try
        {
            context.StudentProfessionals.Add(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }
        catch (Exception ex)
        {
            return new Exception($"Error saving changes: {ex.Message}", ex);
        }
    }

    public Task<Result<Exception, IQueryable<StudentProfessional>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<Exception, StudentProfessional>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Exception, Unit>> UpdateAsync(StudentProfessional entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Exception, Unit>> DeleteAsync(StudentProfessional entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Exception, IQueryable<Student>>> GetAllUserIdByProfessionalId(int professionalId)
    {
        return Task.FromResult(Result<Exception, IQueryable<Student>>.Of(context.StudentProfessionals.AsNoTracking().Where(x => x.ProfessionalId == professionalId).Select(x => x.Student).AsQueryable()));
    }
}