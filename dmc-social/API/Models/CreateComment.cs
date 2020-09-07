namespace DmcSocial.API.Models
{
    public class CreateComment
    {
        public int postId { get; set; }
        public int? commentId { get; set; }
        public string content { get; set; }
    }
}