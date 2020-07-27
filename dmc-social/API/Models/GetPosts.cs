using System.Collections.Generic;

namespace DmcSocial.API.Models
{
    public class GetPosts
    {
        public int? pageIndex { get; set; }
        public int? pageRows { get; set; }
        public string[] tags { get; set; }
    }
}