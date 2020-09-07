using System;

namespace DmcSocial.API.Models
{
    public class Tag
    {
        public string value { get; set; }
        public int postCount { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}