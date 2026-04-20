using Microsoft.EntityFrameworkCore;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Features.Professionals;
using SD_Server.Infra.Data.Context;
using SD_SharedKernel.Helpers;

namespace SD_Server.Infra.Data.Features.Professionals;

public class ProfessionalRepository(SdServerDbContext context) : IProfessionalRepository
{
    public async Task<Result<Exception, int>> AddAsync(Professional entity)
    {
        try
        {
            context.Professionals.Add(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }
        catch (Exception ex)
        {
            return new Exception($"Error saving changes: {ex.Message}", ex);
        }
    }

    public async Task<Result<Exception, Unit>> DeleteAsync(Professional entity)
    {
        try
        {
            var professional = await context.Professionals.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (professional is null)
                return new KeyNotFoundException($"Profissional {entity.Id} não encontrado.");

            professional.Status = StatusEnum.Inactive;
            await context.SaveChangesAsync();
            return Unit.Sucessful;
        }
        catch (Exception ex)
        {
            return new Exception($"Error saving changes: {ex.Message}", ex);
        }
    }

    public Task<Result<Exception, IQueryable<Professional>>> GetAllAsync()
    {
        return Task.FromResult(Result<Exception, IQueryable<Professional>>.Of(context.Professionals.AsNoTracking().AsQueryable()));
    }

    public async Task<Result<Exception, Professional>> GetByIdAsync(int id)
    {
        var professional = await context.Professionals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (professional is null)
            return new KeyNotFoundException($"Profissional {id} não encontrado.");
        return professional;
    }

    public async Task<Result<Exception, Unit>> UpdateAsync(Professional entity)
    {
        try
        {
            var professional = await context.Professionals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (!string.IsNullOrEmpty(entity.Phone))
                professional.Phone = entity.Phone;

            if (!string.IsNullOrEmpty(entity.Email))
                professional.Email = entity.Email;

            if (!string.IsNullOrEmpty(entity.Name))
                professional.Name = entity.Name;

            if (!string.IsNullOrEmpty(entity.Cref))
                professional.Cref = entity.Cref;

            if (!string.IsNullOrEmpty(entity.Bio))
                professional.Bio = entity.Bio;

            context.Professionals.Update(professional);

            await context.SaveChangesAsync();
            return Unit.Sucessful;
        }
        catch (Exception ex)
        {
            return new Exception($"Error saving changes: {ex.Message}", ex);
        }
    }
}
