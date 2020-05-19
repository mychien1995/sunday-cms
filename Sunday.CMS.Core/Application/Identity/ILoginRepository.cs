using Sunday.CMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Application.Identity
{
    public interface ILoginRepository
    {
        Task<LoginApiResponse> LoginAsync(LoginInputModel credential);
    }
}
