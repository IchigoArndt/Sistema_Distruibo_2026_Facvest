using Microsoft.EntityFrameworkCore;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Features.Students;
using SD_Server.Infra.Data.Context;
using SD_SharedKernel.Helpers;

namespace SD_Server.Infra.Data.Features.Students;

public class StudentRepository(SdServerDbContext context) : IStudentRepository
{
    public async Task<Result<Exception, int>> AddAsync(Student entity)
    {
        try
        {
            context.Students.Add(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }
        catch (Exception ex)
        {
            return new Exception($"Error saving changes: {ex.Message}", ex);
        }
    }

    public async Task<Result<Exception, Unit>> DeleteAsync(Student entity)
    {
        try
        {
            var student = await context.Students.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (student is null)
                return new KeyNotFoundException($"Aluno {entity.Id} não encontrado.");

            student.Status = StatusEnum.Inactive;
            await context.SaveChangesAsync();
            return Unit.Sucessful;
        }
        catch (Exception ex)
        {
            return new Exception($"Error saving changes: {ex.Message}", ex);
        }
    }

    public Task<Result<Exception, IQueryable<Student>>> GetAllAsync()
    {
        return Task.FromResult(Result<Exception, IQueryable<Student>>.Of(context.Students.AsNoTracking().AsQueryable()));
    }

    public async Task<Result<Exception, Student>> GetByIdAsync(int id)
    {
        var student = await context.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (student is null)
            return new KeyNotFoundException($"Aluno {id} não encontrado.");
        return student;
    }

    public async Task<Result<Exception, Unit>> UpdateAsync(Student entity)
    {
        try
        {
            context.Students.Update(entity);
            await context.SaveChangesAsync();
            return Unit.Sucessful;
        }
        catch (Exception ex)
        {
            return new Exception($"Error saving changes: {ex.Message}", ex);
        }
    }
}
