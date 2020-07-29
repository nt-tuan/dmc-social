using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using DmcSocial.Models;

namespace DmcSocial.API.Models
{
    public class CommentResponse
    {
        public int id { get; set; }
        public string content { get; set; }
        public int? postId { get; set; }
        public int? commentId { get; set; }
        public DateTime createdAt { get; set; }
        public string createdBy { get; set; }
        public DateTime? lastModifiedAt { get; set; }

        public CommentResponse(PostComment entity)
        {
            id = entity.Id;
            content = entity.Content;
            postId = entity.PostId;
            commentId = entity.ParentPostCommentId;
            createdBy = entity.CreatedBy;
            createdAt = entity.DateCreated;
            lastModifiedAt = entity.LastModifiedTime;
        }
    }
}