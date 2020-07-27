using System;

namespace DmcSocial.Models
{
    public class PostTag
    {
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string TagId { get; set; }
        public Tag Tag { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateRemoved { get; set; }
        public string RemovedBy { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
