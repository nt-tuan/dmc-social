using System;
using System.Collections.Generic;

namespace DmcSocial.API.Models
{
    public class PostCommentResponse
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? PostId { get; set; }
        public int? ParentPostCommentId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }

        public PostCommentResponse(DmcSocial.Models.PostComment entity)
        {
            Id = entity.Id;
            Content = entity.Content;
            PostId = entity.PostId;
            ParentPostCommentId = entity.ParentPostCommentId;
            CreatedAt = entity.DateCreated;
            CreatedBy = entity.CreatedBy;
            LastModifiedAt = entity.LastModifiedTime;
        }
    }
}
