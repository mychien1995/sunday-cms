using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Core.Models;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Entities;
using Sunday.Foundation.Models;

namespace Sunday.Foundation.Application.Repositories
{
    public interface IUserRepository
    {
        Task<SearchResult<UserEntity>> QueryAsync(UserQuery query);

        Task<UserEntity> GetUserByIdAsync(int userId);

        Task<UserEntity> CreateAsync(UserEntity user);

        Task<UserEntity> UpdateAsync(UserEntity user);

        Task<bool> DeleteAsync(int userId);

        Task<bool> ActivateAsync(int userId);

        Task<bool> DeactivateAsync(int userId);

        Task<bool> UpdatePasswordAsync(UserEntity user);
    }
}
