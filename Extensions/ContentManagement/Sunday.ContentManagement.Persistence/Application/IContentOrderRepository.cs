using System;
using System.Threading.Tasks;
using Sunday.ContentManagement.Models;

namespace Sunday.ContentManagement.Persistence.Application
{
    public interface IContentOrderRepository
    {
        Task SaveOrder(ContentOrder[] orders);
        Task<ContentOrder[]> GetOrders(Guid[] contentIds);
    }
}
