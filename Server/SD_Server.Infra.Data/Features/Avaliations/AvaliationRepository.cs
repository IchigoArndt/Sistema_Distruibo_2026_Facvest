using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Features.Avaliations;
using SD_Server.Infra.Data.Context;
using SD_SharedKernel.Helpers;

namespace SD_Server.Infra.Data.Features.Avaliations;

public class AvaliationRepository(SdServerDbContext context, ILogger<AvaliationRepository> logger) : IAvaliationRepository
{
    public async Task<Result<Exception, int>> AddAsync(Avaliation entity)
    {
        try
        {
            logger.LogInformation("Salvando avaliação para o aluno Id: {StudentId}", entity.StudentId);

            context.Avaliations.Add(entity);
            await context.SaveChangesAsync();

            logger.LogInformation("Avaliação criada com sucesso. Id: {Id}", entity.Id);
            return entity.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao salvar avaliação para o aluno Id: {StudentId}", entity.StudentId);
            return new Exception($"Erro ao salvar avaliação: {ex.Message}", ex);
        }
    }

    public async Task<Result<Exception, Unit>> DeleteAsync(Avaliation entity)
    {
        try
        {
            logger.LogInformation("Inativando avaliação Id: {Id}", entity.Id);

            var avaliation = await context.Avaliations.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (avaliation is null)
            {
                logger.LogWarning("Avaliação Id: {Id} não encontrada para inativação.", entity.Id);
                return new KeyNotFoundException($"Avaliação {entity.Id} não encontrada.");
            }

            avaliation.Status = StatusAssessmentEnum.Completed;
            await context.SaveChangesAsync();

            logger.LogInformation("Avaliação Id: {Id} inativada com sucesso.", entity.Id);
            return Unit.Sucessful;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao inativar avaliação Id: {Id}", entity.Id);
            return new Exception($"Erro ao inativar avaliação: {ex.Message}", ex);
        }
    }

    public Task<Result<Exception, IQueryable<Avaliation>>> GetAllAsync()
    {
        logger.LogInformation("Buscando todas as avaliações.");
        return Task.FromResult(
            Result<Exception, IQueryable<Avaliation>>.Of(
                context.Avaliations.AsNoTracking().AsQueryable()));
    }

    public async Task<Result<Exception, Avaliation>> GetByIdAsync(int id)
    {
        logger.LogInformation("Buscando avaliação Id: {Id}", id);

        var avaliation = await context.Avaliations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (avaliation is null)
        {
            logger.LogWarning("Avaliação Id: {Id} não encontrada.", id);
            return new KeyNotFoundException($"Avaliação {id} não encontrada.");
        }

        return avaliation;
    }

    public async Task<Result<Exception, Unit>> UpdateAsync(Avaliation entity)
    {
        try
        {
            logger.LogInformation("Atualizando avaliação Id: {Id}", entity.Id);

            var avaliation = await context.Avaliations.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (avaliation is null)
            {
                logger.LogWarning("Avaliação Id: {Id} não encontrada para atualização.", entity.Id);
                return new KeyNotFoundException($"Avaliação {entity.Id} não encontrada.");
            }

            if (entity.BodyComposition is not null)
                avaliation.BodyComposition = entity.BodyComposition;

            if (entity.Skinfolds is not null)
                avaliation.Skinfolds = entity.Skinfolds;

            if (entity.Anamnesis is not null)
                avaliation.Anamnesis = entity.Anamnesis;

            if (!string.IsNullOrEmpty(entity.TechnicalOpinion))
                avaliation.TechnicalOpinion = entity.TechnicalOpinion;

            if (!string.IsNullOrEmpty(entity.IMC))
                avaliation.IMC = entity.IMC;

            if (!string.IsNullOrEmpty(entity.BodyFatPercentage))
                avaliation.BodyFatPercentage = entity.BodyFatPercentage;

            if (entity.DateNextAvaliation.HasValue)
                avaliation.DateNextAvaliation = entity.DateNextAvaliation;

            context.Avaliations.Update(avaliation);
            await context.SaveChangesAsync();

            logger.LogInformation("Avaliação Id: {Id} atualizada com sucesso.", entity.Id);
            return Unit.Sucessful;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar avaliação Id: {Id}", entity.Id);
            return new Exception($"Erro ao atualizar avaliação: {ex.Message}", ex);
        }
    }
}
