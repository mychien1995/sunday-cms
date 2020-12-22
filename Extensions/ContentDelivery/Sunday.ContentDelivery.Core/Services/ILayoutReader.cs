using System;
using System.Threading.Tasks;

namespace Sunday.ContentDelivery.Core.Services
{
    public interface ILayoutReader
    {
        Task<string?> GetLayout(Guid layoutId);
    }
}
