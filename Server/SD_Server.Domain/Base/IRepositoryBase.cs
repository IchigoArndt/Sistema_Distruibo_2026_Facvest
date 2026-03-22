using SD_SharedKernel.Helpers;

namespace SD_Server.Domain.Base
{
    public interface IRepositoryBase<T> where T: BaseEntity
    {
        Task<Result<Exception, int>> AddAsync(T entity);

        Task<Result<Exception, IQueryable<T>>> GetAllAsync();

        Task<Result<Exception, T>> GetByIdAsync(int id);

        Task<Result<Exception, Unit>> UpdateAsync(T entity);

        Task<Result<Exception, Unit>> DeleteAsync(T entity);
    }
}
