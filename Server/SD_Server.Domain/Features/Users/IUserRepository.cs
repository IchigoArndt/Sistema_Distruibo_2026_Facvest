using SD_Server.Domain.Base;
using SD_Server.Domain.Enum;
using SD_SharedKernel.Helpers;

namespace SD_Server.Domain.Features.Users
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<Result<Exception, User>> GetByEmail(string email);
        Task<Result<Exception, User>> GetByEntityId(int entityId, TypeUserEnum type);
    }
}
