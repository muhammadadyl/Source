using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Journals.Web.ViewModels
{
    public class IssueViewModel
    {
        public int Id { get; set; }

        public int Version { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        [Required, ValidateFile]
        public HttpPostedFileBase File { get; set; }

        public int JournalId { get; set; }
    }
}