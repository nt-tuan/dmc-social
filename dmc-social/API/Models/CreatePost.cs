using System.Linq;
using System;
using System.Collections.Generic;

namespace DmcSocial
{
    public class CreatePost
    {
        public string content { get; set; }
        public string subject { get; set; }
        public bool canComment { get; set; }
        public int postRestrictionType { get; set; }
        public string[] accessUsers { get; set; }
        public string[] tags { get; set; }
        public virtual Models.Post ToEntity()
        {
            var post = new Models.Post();
            post.Content = content;
            post.Subject = subject;
            post.CanComment = canComment;
            post.DateCreated = DateTime.Now;
            if (tags != null)
            {
                post.PostTags = tags.Select(tag => new Models.PostTag
                {
                    TagId = tag
                }).ToList();
            }
            post.PostRestrictionType = postRestrictionType;
            if (accessUsers != null)
            {
                post.PostAccessUsers = accessUsers;
            }
            return post;
        }
    }
}