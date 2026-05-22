using SD_Server.Domain.Features.Assessments;
using SD_Server.Infra.Data.Context;
using SD_SharedKernel.Helpers;
using Microsoft.EntityFrameworkCore;

namespace SD_Server.Infra.Data.Features.Assessments;

public class AssessmentRepository(SdServerDbContext context) : IAssessmentRepository
{
    public async Task<Result<Exception, int>> AddAsync(Assessment entity)
    {
        try
        {
            context.Add(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }
        catch (Exception ex)
        {
            return new Exception($"Error saving assessment: {ex.Message}", ex);
        }
    }

    public Task<Result<Exception, IQueryable<Assessment>>> GetAllAsync()
    {
        return Task.FromResult(Result<Exception, IQueryable<Assessment>>.Of(context.Set<Assessment>().AsNoTracking().AsQueryable()));
    }

    public async Task<Result<Exception, Assessment>> GetByIdAsync(int id)
    {
        var assessment = await context.Set<Assessment>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (assessment is null)
            return new KeyNotFoundException($"Avaliação {id} não encontrada.");
        return assessment;
    }

    public async Task<Result<Exception, Unit>> UpdateAsync(Assessment entity)
    {
        try
        {
            var existing = await context.Set<Assessment>().FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (existing is null) return new KeyNotFoundException($"Avaliação {entity.Id} não encontrada.");

            existing.Date = entity.Date;
            existing.Methodology = entity.Methodology;
            existing.Price = entity.Price;
            existing.Notes = entity.Notes;
            existing.Results = entity.Results;
            existing.Status = entity.Status;

            context.Update(existing);
            await context.SaveChangesAsync();
            return Unit.Sucessful;
        }
        catch (Exception ex)
        {
            return new Exception($"Error updating assessment: {ex.Message}", ex);
        }
    }

    public async Task<Result<Exception, Unit>> DeleteAsync(Assessment entity)
    {
        try
        {
            var existing = await context.Set<Assessment>().FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (existing is null) return new KeyNotFoundException($"Avaliação {entity.Id} não encontrada.");

            context.Remove(existing);
            await context.SaveChangesAsync();
            return Unit.Sucessful;
        }
        catch (Exception ex)
        {
            return new Exception($"Error deleting assessment: {ex.Message}", ex);
        }
    }
}
