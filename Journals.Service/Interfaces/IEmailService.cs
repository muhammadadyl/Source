using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journals.Service.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string username, string email, string subject, string content);
    }
}
