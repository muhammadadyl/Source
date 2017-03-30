using Journals.Model;
using System;

namespace Journals.Model
{
    public class Issue : BaseEntity
    {
        public int Id { get; set; }
        public int version { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int JournalId { get; set; }
        public Journal Journal { get; set; }

        public Issue()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
