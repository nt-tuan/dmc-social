namespace DmcSocial.API.Models
{
    public class Paging
    {
        public int PageIndex { get; set; }
        public int PageRow { get; set; }
        public string OrderBy { get; set; }
        public int OrderDir { get; set; }
    }
}