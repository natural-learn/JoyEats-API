using JoyEats.Common.Constant;
using JoyEats.Core.VO.Dashboard;
using JoyEats.IRepository;
using JoyEats.IRepository.UnitOfWork;
using JoyEats.IServices;
using JoyEats.Models;

namespace JoyEats.Services
{
    public class DashboardService : BaseService, IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDishRepository _dishRepository;
        private readonly ISetmealRepository _setmealRepository;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = _unitOfWork.GetRepository<IOrderRepository>();
            _userRepository = _unitOfWork.GetRepository<IUserRepository>();
            _dishRepository = _unitOfWork.GetRepository<IDishRepository>();
            _setmealRepository = _unitOfWork.GetRepository<ISetmealRepository>();
        }

        /// <summary>
        /// 根据时间段统计营业数据
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<BusinessDataVO> GetBusinessDataAsync(DateTime begin, DateTime end)
        {
            /**
            * 营业额：当日已完成订单的总金额
            * 有效订单：当日已完成订单的数量
            * 订单完成率：有效订单数 / 总订单数
            * 平均客单价：营业额 / 有效订单数
            * 新增用户：当日新增用户的数量
            */

            Dictionary<string, object> map = new Dictionary<string, object>()
            {
                { "begin",begin },
                { "end",end }
            };

            // 查询总订单数
            int totalOrderCount = await _orderRepository.CountByMapAsync(map);
            map.Add("status", OrderStatus.COMPLETED);

            // 营业额
            decimal turnover = await _orderRepository.SumByMapAsync(map);

            //有效订单数
            int validOrderCount = await _orderRepository.CountByMapAsync(map);

            decimal unitPrice = 0.0m;

            double orderCompletionRate = 0.0;
            if (totalOrderCount != 0 && validOrderCount != 0)
            {
                //订单完成率
                orderCompletionRate = validOrderCount / totalOrderCount;
                //平均客单价
                unitPrice = turnover / validOrderCount;
            }

            //新增用户数
            int newUsers = await _userRepository.CountByMapAsync(map);

            return new BusinessDataVO
            {
                Turnover = turnover,
                ValidOrderCount = validOrderCount,
                OrderCompletionRate = orderCompletionRate,
                UnitPrice = unitPrice,
                NewUsers = newUsers
            };
        }

        /// <summary>
        /// 查询订单管理数据
        /// </summary>
        /// <returns></returns>
        public async Task<OrderOverViewVO> GetOrderOverViewAsync()
        {
            Dictionary<string, object> map = new Dictionary<string, object>()
            {
                { "begin", DateTime.Now.Date },
                { "status", OrderStatus.TO_BE_CONFIRMED }
            };

            //待接单
            int waitingOrders = await _orderRepository.CountByMapAsync(map);

            //待派送
            map["status"] = OrderStatus.CONFIRMED;  //在C#中不能使用重复添加的方式来替换对应键的值，会抛出异常
            int deliveredOrders = await _orderRepository.CountByMapAsync(map);

            //已完成
            map["status"] = OrderStatus.COMPLETED;
            int completedOrders = await _orderRepository.CountByMapAsync(map);

            //已取消
            map["status"] = OrderStatus.CANCELLED;
            int cancelledOrders = await _orderRepository.CountByMapAsync(map);

            //全部订单
            map["status"] = null;
            int allOrders = await _orderRepository.CountByMapAsync(map);

            return new OrderOverViewVO
            {
                WaitingOrders = waitingOrders,
                DeliveredOrders = deliveredOrders,
                CompletedOrders = completedOrders,
                CancelledOrders = cancelledOrders,
                AllOrders = allOrders
            };
        }

        /// <summary>
        /// 查询菜品总览
        /// </summary>
        /// <returns></returns>
        public async Task<DishOverViewVO> GetDishOverViewAsync()
        {
            Dictionary<string, object> map = new Dictionary<string, object>()
            {
                { "status", StatusConstant.ENABLE },
            };

            int sold = await _dishRepository.CountByMapAsync(map);

            map["status"] = StatusConstant.DISABLE;
            int discontinued = await _dishRepository.CountByMapAsync(map);

            return new DishOverViewVO
            {
                Sold = sold,
                Discontinued = discontinued
            };
        }

        /// <summary>
        /// 查询套餐总览
        /// </summary>
        /// <returns></returns>
        public async Task<SetmealOverViewVO> GetSetmealOverViewAsync()
        {
            Dictionary<string, object> map = new Dictionary<string, object>()
            {
                { "status", StatusConstant.ENABLE },
            };
            int sold = await _setmealRepository.CountByMapAsync(map);

            map["status"] = StatusConstant.DISABLE;
            int discontinued = await _setmealRepository.CountByMapAsync(map);

            return new SetmealOverViewVO
            {
                Sold = sold,
                Discontinued = discontinued
            };
        }
    }
}
