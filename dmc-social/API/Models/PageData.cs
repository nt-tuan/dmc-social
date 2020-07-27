using System.Collections.Generic;

namespace DmcSocial.API.Models
{
    public class PageData<T>
    {
        public List<T> data { get; set; }
        public int total { get; set; }
        public PageData(List<T> data, int total)
        {
            this.data = data;
            this.total = total;
        }
    }
}