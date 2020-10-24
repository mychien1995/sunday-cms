using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Implementation.Pipelines.Arguments;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Application.Repositories;

namespace Sunday.Foundation.Implementation.Pipelines.Users
{
    public class ValidateUsernameAndEmail : IAsyncPipelineProcessor
    {
        private readonly IUserRepository _userRepository;
        public ValidateUsernameAndEmail(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task ProcessAsync(PipelineArg pipelineArg)
        {
            switch (pipelineArg)
            {
                case BeforeCreateUserArg arg:
                    if (await _userRepository.FindUserByNameAsync(arg.User.UserName).MapResultTo(rs => rs.IsSome))
                    {
                        throw new ArgumentException($"Username {arg.User.UserName} already in used");
                    }
                    else if (await _userRepository.QueryAsync(new UserQuery { Email = arg.User.Email }).MapResultTo(rs => rs.Result.Any()))
                    {
                        throw new ArgumentException($"Email {arg.User.Email} already in used");
                    }
                    break;
                case BeforeUpdateUserArg arg:
                    if (await _userRepository.FindUserByNameAsync(arg.User.UserName).MapResultTo(rs => rs.IsSome && rs.Get().Id != arg.User.Id))
                    {
                        throw new ArgumentException($"Username {arg.User.UserName} already in used");
                    }
                    else if (await _userRepository.QueryAsync(new UserQuery { Email = arg.User.Email }).MapResultTo(rs => rs.Result.Any(u => u.Id != arg.User.Id)))
                    {
                        throw new ArgumentException($"Email {arg.User.Email} already in used");
                    }
                    break;
            }
        }
    }
}
