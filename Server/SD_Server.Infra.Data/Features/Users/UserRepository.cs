using Microsoft.EntityFrameworkCore;
using SD_Server.Domain.Enum;
using SD_Server.Domain.Features.Users;
using SD_Server.Infra.Data.Context;
using SD_SharedKernel.Helpers;

namespace SD_Server.Infra.Data.Features.Users
{
    public class UserRepository(SdServerDbContext context) : IUserRepository
    {
        public async Task<Result<Exception, int>> AddAsync(User entity)
        {
            try
            {
                context.Users.Add(entity);
                await context.SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception ex)
            {
                return new Exception($"Error saving changes: {ex.Message}", ex);
            }
        }

        public async Task<Result<Exception, Unit>> DeleteAsync(User entity)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == entity.Id);
                if (user is null)
                    return new KeyNotFoundException($"Usuario {entity.Id} não encontrado.");

                user.Status = StatusEnum.Inactive;
                await context.SaveChangesAsync();
                return Unit.Sucessful;
            }
            catch (Exception ex)
            {
                return new Exception($"Error saving changes: {ex.Message}", ex);
            }
        }

        public Task<Result<Exception, IQueryable<User>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Exception, User>> GetByEmail(string email)
        {
            var user = context.Users.AsNoTracking().FirstOrDefault(x => x.Email == email);

            if (user is null)
                return new KeyNotFoundException($"Usuario não encontrado.");

            return user;
        }

        public async Task<Result<Exception, User>> GetByEntityId(int entityId, TypeUserEnum type)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.EntityId == entityId && x.TypeAccess == type);

            if (user is null)
                return new KeyNotFoundException($"Usuário vinculado não encontrado para o EntityId {entityId}.");

            return user;
        }

        public Task<Result<Exception, User>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Exception, Unit>> UpdateAsync(User entity)
        {
            try
            {
                var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);

                if (!string.IsNullOrEmpty(entity.PasswordHash))
                    user.PasswordHash = entity.PasswordHash;

                context.Users.Update(user);

                await context.SaveChangesAsync();
                return Unit.Sucessful;
            }
            catch (Exception ex)
            {
                return new Exception($"Error saving changes: {ex.Message}", ex);
            }
        }
    }
}
