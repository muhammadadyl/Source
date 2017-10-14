using System;

namespace Journals.Model
{
    public class Issue : BaseEntity
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int JournalId { get; set; }
        public Journal Journal { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Issue()
        {
            ModifiedDate = DateTime.Now;
            CreatedDate = DateTime.Now;
        }
    }
}
