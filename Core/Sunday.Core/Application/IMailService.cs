using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sunday.Core.Application
{
    public interface IMailService
    {
        Task SendEmail(string subject, string template, List<string> recipients, params string[] datas);
    }
}
