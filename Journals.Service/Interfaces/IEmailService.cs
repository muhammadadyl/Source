using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journals.Service.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Dictionary<string, string> emails,string subject, string content);
    }
}
