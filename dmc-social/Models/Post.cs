using System;
using System.Collections.Generic;

namespace DmcSocial.Models
{
    public class Post : BaseEntity
    {
        public enum PostRestriction { NONE, ALLOW_USERS }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool CanComment { get; set; }

        //META DATA
        public DateTime LastModifiedDate { get; set; }
        public int ViewCount { get; set; }
        public int CommentCount { get; set; }

        //REFERENCES    
        public ICollection<PostComment> Comments { get; set; }
        public int PostRestrictionType { get; set; }
        public string[] PostAccessUsers { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
    }
}
