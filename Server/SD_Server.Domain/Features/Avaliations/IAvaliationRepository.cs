using SD_Server.Domain.Base;
using SD_SharedKernel.Helpers;

namespace SD_Server.Domain.Features.Avaliations
{
    public interface IAvaliationRepository : IRepositoryBase<Avaliation>
    {
        Task<Result<Exception, IQueryable<Avaliation>>> GetByStudentIdAsync(int studentId);
    }
}
