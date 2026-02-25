namespace JoyEats.Core.VO.Order
{
    public class OrdersConfirmDTO
    {
        public long Id { get; set; }

        //订单状态 1待付款 2待接单 3 已接单 4 派送中 5 已完成 6 已取消 7 退款
        public int Status { get; set; }
    }
}
