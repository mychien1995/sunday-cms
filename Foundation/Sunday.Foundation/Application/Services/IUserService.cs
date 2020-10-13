using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Models;

namespace Sunday.Foundation.Application.Services
{
    public interface IUserService
    {
        Task<SearchResult<ApplicationUser>> QueryAsync(UserQuery query);

        Task<Option<ApplicationUser>> GetUserByIdAsync(Guid userId);

        Task<Guid> CreateAsync(ApplicationUser user);

        Task UpdateAsync(ApplicationUser user);

        Task DeleteAsync(Guid userId);

        Task ActivateAsync(Guid userId);

        Task DeactivateAsync(Guid userId);

        Task UpdatePasswordAsync(ApplicationUser user);
    }
}
