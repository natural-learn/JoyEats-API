using JoyEats.Models;

namespace JoyEats.Core.VO.Order
{
    public class OrderVO
    {
        /// <summary>
        /// 订单菜品信息
        /// </summary>
        public string OrderDishes { get; set; }

        /// <summary>
        /// 订单详情
        /// </summary>
        public List<OrderDetail> OrderDetailList { get; set; }
    }
}
