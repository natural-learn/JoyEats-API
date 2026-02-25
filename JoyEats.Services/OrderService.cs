using JoyEats.Common;
using JoyEats.Common.Constant;
using JoyEats.Core.DTO.Order;
using JoyEats.Core.Exceptions;
using JoyEats.Core.VO.Order;
using JoyEats.IRepository;
using JoyEats.IRepository.UnitOfWork;
using JoyEats.IServices;
using JoyEats.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace JoyEats.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserContextService _currentUserContextService;
        private readonly ILogger<OrderService> _logger;
        private readonly IBaseRepository<AddressBook> _addressBookRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderService(IUnitOfWork unitOfWork, ICurrentUserContextService currentUserContextService, ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserContextService = currentUserContextService;
            _logger = logger;
            _addressBookRepository = _unitOfWork.GetBaseRepository<AddressBook>();
            _userRepository = _unitOfWork.GetBaseRepository<User>();
            _shoppingCartRepository = _unitOfWork.GetRepository<IShoppingCartRepository>();
            _orderRepository = _unitOfWork.GetRepository<IOrderRepository>();
            _orderDetailRepository = _unitOfWork.GetRepository<IOrderDetailRepository>();
        }

        /// <summary>
        /// 用户下单
        /// </summary>
        /// <param name="ordersSubmitDTO"></param>
        /// <returns></returns>
        public async Task<OrderSubmitVO> SubmitOrderAsync(OrdersSubmitDTO ordersSubmitDTO)
        {
            var userId = await _currentUserContextService.GetCurrentUserIdAsync();
            AddressBook? addressBook = await _addressBookRepository.GetByIdAsync(ordersSubmitDTO.AddressBookId) ??
                throw new BusinessException(MessageConstant.ADDRESS_BOOK_IS_NULL);
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                UserId = userId,
            };

            // 查询当前用户的购物车数据
            List<ShoppingCart> shoppingCartList = await _shoppingCartRepository.ListAsync(shoppingCart);
            if (shoppingCartList == null || shoppingCartList.Count == 0)
            {
                throw new BusinessException(MessageConstant.SHOPPING_CART_IS_NULL);
            }

            // 构造订单数据
            Orders orders = Mapper.Map<Orders>(ordersSubmitDTO);
            orders.Phone = addressBook.Phone;
            orders.Address = addressBook.Detail;
            orders.Consignee = addressBook.Consignee;
            orders.Number = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            orders.UserId = userId;
            orders.Status = OrderStatus.PENDING_PAYMENT;
            orders.PayStatus = OrderStatus.UN_PAID;
            orders.OrderTime = DateTime.Now;

            await _orderRepository.AddAsync(orders);
            await _unitOfWork.SaveChangesAsync();   // 确保下面能正确获取到orders.Id

            // 订单明细
            List<OrderDetail> orderDetailList = new List<OrderDetail>();
            foreach (ShoppingCart sc in shoppingCartList)
            {
                OrderDetail orderDetail = Mapper.Map<OrderDetail>(sc);
                // 这里要获取到更新到数据库之后的orders.Id，不然为默认的0
                orderDetail.OrderId = orders.Id;
                orderDetail.DishFlavor = sc.DishFlavor ?? string.Empty;
                orderDetailList.Add(orderDetail);
            }

            await _orderDetailRepository.AddRangeAsync(orderDetailList);

            await _shoppingCartRepository.GetQueryable().Where(sc => sc.UserId == userId).ExecuteDeleteAsync();

            OrderSubmitVO orderSubmitVO = new OrderSubmitVO()
            {
                Id = orders.Id,
                OrderNumber = orders.Number,
                OrderAmount = orders.Amount,
                OrderTime = orders.OrderTime,
            };

            await _unitOfWork.SaveChangesAsync();
            return orderSubmitVO;
        }

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="ordersPaymentDTO"></param>
        /// <returns></returns>
        public async Task<OrderPaymentVO> PaymentAsync(OrdersPaymentDTO ordersPaymentDTO)
        {
            var userId = await _currentUserContextService.GetCurrentUserIdAsync();
            User? user = await _userRepository.GetByIdAsync(userId) ??
                throw new EntityNotFoundException($"未找到Id为{userId}的用户");
            return new OrderPaymentVO();
        }

        /// <summary>
        /// 历史订单查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="status">订单状态 1待付款 2待接单 3已接单 4派送中 5已完成 6已取消</param>
        /// <returns></returns>
        public async Task<PagedResult<OrderVO>> PageQueryUserAsync(int page, int pageSize, int? status)
        {
            var userId = await _currentUserContextService.GetCurrentUserIdAsync();

            Expression<Func<Orders, bool>> predicate = o => o.UserId == userId;

            if (status.HasValue)
            {
                predicate = predicate.And(o => o.Status == status.Value);
            }

            PagedResult<Orders> pagedResult = await _orderRepository
                .GetPagedListAsync(
                    predicate,
                    o => o.OrderTime,
                    page,
                    pageSize,
                    false);

            if (pagedResult == null || pagedResult.Total == 0)
            {
                return PagedResult<OrderVO>.GetPagedResult(new List<OrderVO>(), page, pageSize, 0);
            }

            // 批量查询所有订单的详情（避免N+1查询）
            var orderIds = pagedResult.Records.Select(o => o.Id).ToList();
            var allOrderDetails = await _orderDetailRepository.GetListAsync(od => orderIds.Contains(od.OrderId));
            var orderDetailsDict = allOrderDetails.GroupBy(od => od.OrderId)
                                                 .ToDictionary(g => g.Key, g => g.ToList());

            // 转换为 OrderVO
            List<OrderVO> orderVOList = new List<OrderVO>();
            foreach (var order in pagedResult.Records)
            {
                orderDetailsDict.TryGetValue(order.Id, out var orderDetailList);

                OrderVO orderVO = new OrderVO()
                {
                    OrderDishes = order.Remark,
                    OrderDetailList = orderDetailList ?? new List<OrderDetail>(),
                };
                orderVOList.Add(orderVO);
            }
            return PagedResult<OrderVO>.GetPagedResult(
                orderVOList, 
                page, 
                pageSize, 
                pagedResult.Total);
        }

        /// <summary>
        /// 查询订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderVO> DetailsAsync(long id)
        {
            Orders orders = await _orderRepository.GetByIdAsync(id) ??
                throw new OrderBusinessException(MessageConstant.ORDER_NOT_FOUND);
            List<OrderDetail> orderDetailList = await _orderDetailRepository.GetListAsync(od => od.OrderId == orders.Id);
            OrderVO orderVO = Mapper.Map<OrderVO>(orders);
            orderVO.OrderDetailList = orderDetailList;
            return orderVO;
        }

        /// <summary>
        /// 用户取消订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task UserCancelByIdAsync(long id)
        {
            Orders orders = await _orderRepository.GetByIdAsync(id) ??
                throw new OrderBusinessException(MessageConstant.ORDER_NOT_FOUND);

            //订单状态 1待付款 2待接单 3已接单 4派送中 5已完成 6已取消
            if (orders.Status > 2)
            {
                throw new OrderBusinessException(MessageConstant.ORDER_STATUS_ERROR);
            }

            // 如果订单处于待接单状态下取消,需要进行退款
            if (orders.Status == OrderStatus.TO_BE_CONFIRMED)
            {
                // 调用微信支付退款接口
                // ......
                // 支付状态修改为退款
                orders.PayStatus = OrderStatus.REFUND;
            }

            // 更新订单状态、取消原因、取消时间
            orders.Status = OrderStatus.CANCELLED;
            orders.CancelReason = "用户取消";
            orders.CancelTime = DateTime.Now;
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 再来一单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RepetitionAsync(long id)
        {
            long userId = await _currentUserContextService.GetCurrentUserIdAsync();

            List<OrderDetail> orderDetailList = await _orderDetailRepository.GetListAsync(od => od.OrderId == id);
            List<ShoppingCart> shoppingCartList = orderDetailList
                .Select(od => Mapper.Map<ShoppingCart>(od))
                .ToList();

            await _shoppingCartRepository.AddRangeAsync(shoppingCartList);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 订单搜索
        /// </summary>
        /// <param name="ordersPageQueryDTO"></param>
        /// <returns></returns>
        public async Task<PagedResult<OrderVO>> ConditionSearchAsync(OrdersPageQueryDTO ordersPageQueryDTO)
        {
            var userId = await _currentUserContextService.GetCurrentUserIdAsync();
            ordersPageQueryDTO.UserId = userId;
            PagedResult<Orders> pagedResult = await _orderRepository.PageQueryAsync(ordersPageQueryDTO);
            List<OrderVO> orderVOList = pagedResult.Records
                .Select(o => Mapper.Map<OrderVO>(o))
                .ToList();
            return PagedResult<OrderVO>.GetPagedResult(orderVOList, ordersPageQueryDTO.Page, ordersPageQueryDTO.PageSize, pagedResult.Total);
        }

        /// <summary>
        /// 各个状态的订单数量统计
        /// </summary>
        /// <returns></returns>
        public async Task<OrderStatisticsVo> StatisticsAsync()
        {
            // 根据状态，分别查询出待接单、待派送、派送中的订单数量
            int toBeConfirmed = await _orderRepository.CountAsync(o => o.Status == OrderStatus.TO_BE_CONFIRMED);
            int confirmed = await _orderRepository.CountAsync(o => o.Status == OrderStatus.CONFIRMED);
            int deliveryInProgress = await _orderRepository.CountAsync(o => o.Status == OrderStatus.DELIVERY_IN_PROGRESS);

            // 将查询出的数据封装到orderStatisticsVO中响应
            OrderStatisticsVo orderStatisticsDto = new OrderStatisticsVo();
            orderStatisticsDto.ToBeConfirmed = toBeConfirmed;
            orderStatisticsDto.Confirmed = confirmed;
            orderStatisticsDto.DeliveryInProgress = deliveryInProgress;
            return orderStatisticsDto;
        }

        /// <summary>
        /// 接单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task ConfirmAsync(OrdersConfirmDTO ordersConfirmDTO)
        {
            Orders orders = new Orders
            {
                Id = ordersConfirmDTO.Id,
                Status = OrderStatus.CONFIRMED,
            };
            _orderRepository.Update(orders);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 拒单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task RejectionAsync(OrdersRejectionDTO ordersRejectionDTO)
        {
            // 根据id查询订单
            Orders? ordersDB = await _orderRepository.GetByIdAsync(ordersRejectionDTO.Id)
                ?? throw new OrderBusinessException(MessageConstant.ORDER_NOT_FOUND);

            // 订单只有存在且状态为2（待接单）才可以拒单
            if (ordersDB == null || !(ordersDB.Status == OrderStatus.TO_BE_CONFIRMED))
            {
                throw new OrderBusinessException(MessageConstant.ORDER_STATUS_ERROR);
            }

            // 支付状态
            int payStatus = ordersDB.PayStatus;
            if (payStatus == OrderStatus.PAID)
            {
                // 用户已支付，拒单需要退款
                // ...
                _logger.LogInformation("申请退款：");
            }

            // 拒单需要退款，根据订单id更新订单状态、拒单原因、取消时间
            Orders orders = new Orders
            {
                Id = ordersDB.Id,
                Status = OrderStatus.CANCELLED,
                RejectionReason = ordersRejectionDTO.RejectionReason,
                CancelTime = DateTime.Now
            };

            _orderRepository.Update(orders);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
