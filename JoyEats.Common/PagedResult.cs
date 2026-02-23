namespace JoyEats.Common
{
    public class PagedResult<T>
    {
        /// <summary>
        /// 当前页数据
        /// </summary>
        public IList<T> Records { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 当前页码（从1开始）
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }

        public static PagedResult<T> GetPagedResult(IList<T> values, int pageIndex,int pageSize, int totalCount)
        {
            return new PagedResult<T>
            {
                Records = values,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = totalCount
            };
        }
    }
}
