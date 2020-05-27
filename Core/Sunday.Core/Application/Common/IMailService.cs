using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.Application.Common
{
    public interface IMailService
    {
        Task SendEmail(string subject, string template, List<string> recipients, params string[] datas);
    }
}
