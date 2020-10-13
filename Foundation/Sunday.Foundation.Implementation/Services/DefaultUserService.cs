using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Application.Repositories;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Implementation.Services
{
    public class DefaultUserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public DefaultUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<SearchResult<ApplicationUser>> QueryAsync(UserQuery query)
            => _userRepository.QueryAsync(query).MapResultTo(rs => rs.CloneTo<ApplicationUser>());

        public Task<Option<ApplicationUser>> GetUserByIdAsync(Guid userId)
            => _userRepository.GetUserByIdAsync(userId)
                .MapResultTo(org => org.Map(o => o.MapTo<ApplicationUser>()));

        public Task<Guid> CreateAsync(ApplicationUser user)
            => _userRepository.CreateAsync(user.MapTo<UserEntity>());

        public Task UpdateAsync(ApplicationUser user)
            => _userRepository.UpdateAsync(user.MapTo<UserEntity>());

        public Task DeleteAsync(Guid userId)
            => _userRepository.DeleteAsync(userId);

        public Task ActivateAsync(Guid userId)
            => _userRepository.ActivateAsync(userId);

        public Task DeactivateAsync(Guid userId)
            => _userRepository.DeactivateAsync(userId);

        public Task UpdatePasswordAsync(ApplicationUser user)
            => _userRepository.UpdatePasswordAsync(user.MapTo<UserEntity>());
    }
}
