using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core.Models;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Persistence.Application.Repositories
{
    public interface IUserRepository
    {
        Task<SearchResult<UserEntity>> QueryAsync(UserQuery query);

        Task<Option<UserEntity>> GetUserByIdAsync(Guid userId);

        Task<Guid> CreateAsync(UserEntity user);

        Task UpdateAsync(UserEntity user);

        Task DeleteAsync(Guid userId);

        Task ActivateAsync(Guid userId);

        Task DeactivateAsync(Guid userId);

        Task UpdatePasswordAsync(UserEntity user);

        Task<Option<UserEntity>> FindUserByNameAsync(string userName);
    }
}
