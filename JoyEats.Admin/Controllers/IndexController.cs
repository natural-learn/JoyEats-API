using JoyEats.Common;
using JoyEats.IRepository;
using JoyEats.IRepository.UnitOfWork;
using JoyEats.IServices;
using JoyEats.Models;
using Microsoft.AspNetCore.Mvc;

namespace JoyEats.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ILogger<IndexController> _logger;
        private readonly ICurrentUserContextService _currentUserContextService;

        public IndexController(IUnitOfWork unitOfWork, 
            IOrderRepository orderRepository, 
            IOrderDetailRepository orderDetailRepository, 
            ILogger<IndexController> logger,
            ICurrentUserContextService currentUserContextService)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _logger = logger;
            _currentUserContextService = currentUserContextService;
        }

        [HttpGet("insertOrders")]
        public async Task<ActionResult<ApiResult>> InsertOrders()
        {
            List<Orders> orders = new List<Orders>()
            {
                new Orders { Number = "ORDER20260227001", Status = OrderStatus.TO_BE_CONFIRMED, UserId = 1001, AddressBookId = 2001, OrderTime = DateTime.Parse("2026-02-27 10:05:30"), CheckoutTime = null, PayMethod = 1, PayStatus = OrderStatus.PAID, Amount = 88.50m, Remark = "少放辣，谢谢", Phone = "13800138001", Address = "北京市朝阳区建国路88号院1号楼2单元301", UserName = "张三", Consignee = "张三", CancelReason = null, CancelTime = null, EstimatedDeliveryTime = DateTime.Parse("2026-02-27 10:30:00"), DeliveryStatus = true, DeliveryTime = null, PackAmount = 5, TablewareNumber = 3, TablewareStatus = true },
                new Orders { Number = "ORDER20260227002", Status = OrderStatus.CONFIRMED, UserId = 1002, AddressBookId = 2002, OrderTime = DateTime.Parse("2026-02-27 09:15:20"), CheckoutTime = DateTime.Parse("2026-02-27 09:18:10"), PayMethod = 2, PayStatus = OrderStatus.PAID, Amount = 128.00m, Remark = "", Phone = "13900139002", Address = "上海市浦东新区张江高科技园区博云路2号", UserName = "李四", Consignee = "李四", CancelReason = null, CancelTime = null, EstimatedDeliveryTime = DateTime.Parse("2026-02-27 09:40:00"), DeliveryStatus = true, DeliveryTime = null, PackAmount = 8, TablewareNumber = 5, TablewareStatus = true },
                new Orders { Number = "ORDER20260227003", Status = OrderStatus.DELIVERY_IN_PROGRESS, UserId = 1003, AddressBookId = 2003, OrderTime = DateTime.Parse("2026-02-27 11:20:15"), CheckoutTime = DateTime.Parse("2026-02-27 11:22:40"), PayMethod = 1, PayStatus = OrderStatus.PAID, Amount = 66.80m, Remark = "放门口即可，不用敲门", Phone = "13700137003", Address = "广州市天河区天河路385号壬丰大厦15层", UserName = "王五", Consignee = "王五", CancelReason = null, CancelTime = null, EstimatedDeliveryTime = DateTime.Parse("2026-02-27 11:45:00"), DeliveryStatus = true, DeliveryTime = null, PackAmount = 3, TablewareNumber = 2, TablewareStatus = true },
                new Orders { Number = "ORDER20260227004", Status = OrderStatus.COMPLETED, UserId = 1004, AddressBookId = 2004, OrderTime = DateTime.Parse("2026-02-27 08:30:00"), CheckoutTime = DateTime.Parse("2026-02-27 08:32:10"), PayMethod = 1, PayStatus = OrderStatus.PAID, Amount = 99.90m, Remark = "", Phone = "13600136004", Address = "深圳市南山区科技园南区科苑路11号", UserName = "赵六", Consignee = "赵六", CancelReason = null, CancelTime = null, EstimatedDeliveryTime = DateTime.Parse("2026-02-27 08:50:00"), DeliveryStatus = true, DeliveryTime = DateTime.Parse("2026-02-27 08:55:00"), PackAmount = 6, TablewareNumber = 4, TablewareStatus = true },
                new Orders { Number = "ORDER20260227005", Status = OrderStatus.CANCELLED, UserId = 1005, AddressBookId = 2005, OrderTime = DateTime.Parse("2026-02-27 12:10:00"), CheckoutTime = null, PayMethod = 2, PayStatus = OrderStatus.UN_PAID, Amount = 78.00m, Remark = "", Phone = "13500135005", Address = "杭州市西湖区文三路478号华星时代广场", UserName = "孙七", Consignee = "孙七", CancelReason = "用户临时改变主意，取消订单", CancelTime = DateTime.Parse("2026-02-27 12:15:00"), EstimatedDeliveryTime = null, DeliveryStatus = true, DeliveryTime = null, PackAmount = 4, TablewareNumber = 3, TablewareStatus = true },
                new Orders { Number = "ORDER20260227006", Status = OrderStatus.PENDING_PAYMENT, UserId = 1006, AddressBookId = 2006, OrderTime = DateTime.Parse("2026-02-27 13:00:00"), CheckoutTime = null, PayMethod = 1, PayStatus = OrderStatus.UN_PAID, Amount = 158.60m, Remark = "尽快配送", Phone = "13400134006", Address = "成都市武侯区锦悦西路2号环球中心", UserName = "周八", Consignee = "周八", CancelReason = null, CancelTime = null, EstimatedDeliveryTime = DateTime.Parse("2026-02-27 13:30:00"), DeliveryStatus = true, DeliveryTime = null, PackAmount = 10, TablewareNumber = 6, TablewareStatus = false },
                new Orders { Number = "ORDER20260227007", Status = OrderStatus.COMPLETED, UserId = 1007, AddressBookId = 2007, OrderTime = DateTime.Parse("2026-02-27 07:45:00"), CheckoutTime = DateTime.Parse("2026-02-27 07:48:20"), PayMethod = 2, PayStatus = OrderStatus.PAID, Amount = 58.00m, Remark = "多加醋", Phone = "13300133007", Address = "重庆市渝中区解放碑青年路18号商社大厦", UserName = "吴九", Consignee = "吴九", CancelReason = null, CancelTime = null, EstimatedDeliveryTime = DateTime.Parse("2026-02-27 08:10:00"), DeliveryStatus = true, DeliveryTime = DateTime.Parse("2026-02-27 08:05:00"), PackAmount = 2, TablewareNumber = 1, TablewareStatus = true },
                new Orders { Number = "ORDER20260227008", Status = OrderStatus.DELIVERY_IN_PROGRESS, UserId = 1008, AddressBookId = 2008, OrderTime = DateTime.Parse("2026-02-27 14:25:00"), CheckoutTime = DateTime.Parse("2026-02-27 14:28:00"), PayMethod = 1, PayStatus = OrderStatus.PAID, Amount = 108.80m, Remark = "", Phone = "13200132008", Address = "武汉市洪山区珞喻路1037号华中科技大学", UserName = "郑十", Consignee = "郑十", CancelReason = null, CancelTime = null, EstimatedDeliveryTime = DateTime.Parse("2026-02-27 14:50:00"), DeliveryStatus = true, DeliveryTime = null, PackAmount = 7, TablewareNumber = 4, TablewareStatus = true }
            };
            await _orderRepository.AddRangeAsync(orders);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Orders[0].id = {@long}", orders[0].Id);
            Console.WriteLine($"Orders[0].Id: {orders[0].Id}");

            List<OrderDetail> testOrderDetails = new List<OrderDetail>()
            {
                new OrderDetail { Name = "麻辣香锅（中辣）", Image = "https://xxx.com/img/mlxg.jpg", OrderId = orders[0].Id, DishId = 1, SetmealId = null, DishFlavor = "中辣", Number = 1, Amount = 68.00m },
                new OrderDetail { Name = "番茄炒蛋", Image = "https://xxx.com/img/fqcd.jpg", OrderId = orders[0].Id, DishId = 2, SetmealId = null, DishFlavor = "微甜", Number = 2, Amount = 28.00m },
                new OrderDetail { Name = "青椒土豆丝", Image = "https://xxx.com/img/qjtds.jpg", OrderId = orders[0].Id, DishId = 3, SetmealId = null, DishFlavor = "不辣", Number = 1, Amount = 18.00m },
                new OrderDetail { Name = "水煮鱼（特辣）", Image = "https://xxx.com/img/szy.jpg", OrderId = orders[0].Id, DishId = 4, SetmealId = null, DishFlavor = "特辣+少麻", Number = 1, Amount = 88.00m },
                new OrderDetail { Name = "宫保鸡丁", Image = "https://xxx.com/img/gbjd.jpg", OrderId = orders[0].Id, DishId = 5, SetmealId = null, DishFlavor = "酸甜口", Number = 1, Amount = 42.00m },
                new OrderDetail { Name = "红烧肉", Image = "https://xxx.com/img/hsr.jpg", OrderId = orders[0].Id, DishId = 6, SetmealId = null, DishFlavor = "酱香", Number = 1, Amount = 58.00m },
                new OrderDetail { Name = "清炒时蔬", Image = "https://xxx.com/img/qcss.jpg", OrderId = orders[0].Id, DishId = 7, SetmealId = null, DishFlavor = "清淡", Number = 1, Amount = 16.00m },
                new OrderDetail { Name = "酸菜鱼", Image = "https://xxx.com/img/scy.jpg", OrderId = orders[0].Id, DishId = 8, SetmealId = null, DishFlavor = "微辣+酸菜多", Number = 1, Amount = 78.00m }
            };

            await _orderDetailRepository.AddRangeAsync(testOrderDetails);
            await _unitOfWork.SaveChangesAsync();
            return Ok("添加订单数据成功");
        }

        [HttpGet]
        public async Task<ActionResult<ApiResult>> Get()
        {
            var orders = await _orderRepository.GetListAsync();
            return Ok(ApiResultHelper.Success(orders));
        }
    }
}
