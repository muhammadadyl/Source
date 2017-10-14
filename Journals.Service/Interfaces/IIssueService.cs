using Journals.Core.Common;
using Journals.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journals.Service.Interfaces
{
    public interface IIssueService
    {
        Issue GetIssueById(int id);
        OperationStatus AddIssue(Issue newIssue);
        OperationStatus UpdateIssue(Issue newIssue);
        OperationStatus DeleteIssue(Issue newIssue);
    }
}
