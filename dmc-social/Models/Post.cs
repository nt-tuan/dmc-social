using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DmcSocial.Models
{
    public class Post : BaseEntity
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool CanComment { get; set; }
        public PostConfig Config { get; set; }
        //META DATA
        public DateTime LastModifiedDate { get; set; }
        public int ViewCount { get; set; }
        public int CommentCount { get; set; }

        //REFERENCES    
        public ICollection<PostComment> Comments { get; set; }

        public ICollection<PostTag> PostTags { get; set; }
    }

    [Owned]
    public class PostConfig
    {
        public enum PostRestriction { NONE, ALLOW_USERS }
        public int PostRestrictionType { get; set; }
        public string[] PostAccessUsers { get; set; }
        public PostRestriction GetPostRestriction()
        {
            return (PostRestriction)PostRestrictionType;
        }

        public void SetPostRestriction(PostRestriction restriction)
        {
            PostRestrictionType = (int)restriction;
        }
    }
}
