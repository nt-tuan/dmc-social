using System;
using System.Collections.Generic;

namespace DmcSocial
{
    public class UpdatePost
    {
        public string content { get; set; }
        public string subject { get; set; }
        public bool canComment { get; set; }
        public int postRestrictionType { get; set; }
        public string[] accessUsers { get; set; }
    }
}