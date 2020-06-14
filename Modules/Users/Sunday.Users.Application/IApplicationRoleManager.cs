using Sunday.Users.Core.Models;
using System.Threading.Tasks;

namespace Sunday.Users.Application
{
    public interface IApplicationRoleManager
    {
        Task<RoleListJsonResult> GetAvailableRoles();
        Task<RoleDetailJsonResult> GetRoleById(int id);
    }
}
