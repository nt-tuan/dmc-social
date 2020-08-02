using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DmcSocial.API.Models
{
    public class PostResponse
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool CanComment { get; set; }

        //META DATA
        public DateTime LastModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ViewCount { get; set; }
        public int CommentCount { get; set; }

        //REFERENCES    
        public int PostRestrictionType { get; set; }
        public string[] PostAccessUsers { get; set; }
        public string[] Tags { get; set; }
        public PostResponse(DmcSocial.Models.Post entity)
        {
            Id = entity.Id;
            Subject = entity.Subject;
            Content = entity.Content;
            CanComment = entity.CanComment;
            LastModifiedAt = entity.LastModifiedDate;
            CreatedBy = entity.CreatedBy;
            CreatedAt = entity.DateCreated;
            ViewCount = entity.ViewCount;
            CommentCount = entity.CommentCount;
            PostRestrictionType = entity.Config.PostRestrictionType;
            PostAccessUsers = entity.Config.PostAccessUsers;
            if (entity.PostTags != null)
            {
                Tags = entity.PostTags.Select(u => u.TagId).ToArray();
            }
        }
    }
}