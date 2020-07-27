using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DmcSocial.Models
{
    public class GetListParams<T>
    {
        public enum OrderDir { ASC = 0, DESC = 1 }
        const string DEFAULT_ORDER_BY = "id";
        public int page { get; set; } = 0;
        public int pageRows { get; set; } = 30;
        public Expression<Func<T, object>> orderBy { get; set; } = null;
        public OrderDir orderDir { get; set; } = OrderDir.ASC;
        public DateTime at { get; set; } = DateTime.Now;
        public GetListParams() { }
        public GetListParams(int? page, int? pageRows)
        {
            this.page = page ?? 0;
            this.pageRows = pageRows ?? 30;
        }

        public int GetSkipNumber()
        {
            return page * pageRows;
        }

        public void SetGetAllItems()
        {
            page = 0;
            pageRows = int.MaxValue;
        }
    }
}
