namespace SkyTakeOut.Common.Constant
{
    /// <summary>
    /// 信息提示常量类
    /// </summary>
    public class MessageConstant
    {
        public const string PASSWORD_ERROR = "密码错误";
        public const string ACCOUNT_NOT_FOUND = "账号不存在";
        public const string ACCOUNT_LOCKED = "账号被锁定";
        public const string UNKNOWN_ERROR = "未知错误";
        public const string USER_NOT_LOGIN = "用户未登录";
        public const string CATEGORY_BE_RELATED_BY_SETMEAL = "当前分类关联了套餐,不能删除";
        public const string CATEGORY_BE_RELATED_BY_DISH = "当前分类关联了菜品,不能删除";
        public const string SHOPPING_CART_IS_NULL = "购物车数据为空，不能下单";
        public const string ADDRESS_BOOK_IS_NULL = "用户地址为空，不能下单";
        public const string LOGIN_FAILED = "登录失败";
        public const string UPLOAD_FAILED = "文件上传失败";
        public const string SETMEAL_ENABLE_FAILED = "套餐内包含未启售菜品，无法启售";
        public const string PASSWORD_EDIT_FAILED = "密码修改失败";
        public const string DISH_ON_SALE = "起售中的菜品不能删除";
        public const string SETMEAL_ON_SALE = "起售中的套餐不能删除";
        public const string DISH_BE_RELATED_BY_SETMEAL = "当前菜品关联了套餐,不能删除";
        public const string ORDER_STATUS_ERROR = "订单状态错误";
        public const string ORDER_NOT_FOUND = "订单不存在";
        public const string ALREADY_EXISTS = "已存在";
    }
}
