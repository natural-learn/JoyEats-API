namespace SkyTakeOut.Common
{
    /// <summary>
    /// 响应模型帮助类
    /// </summary>
    public class ApiResultHelper
    {
        /// <summary>
        /// 创建成功响应（带数据）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ApiResult<T> Success<T>(T data, string message = "操作成功", int code = 1)
        {
            return new ApiResult<T> { Code = code, Success = true, Message = message, Data = data };
        }

        /// <summary>
        /// 创建成功响应（无数据）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ApiResult Success(string message = "操作成功", int code = 1)
        {
            return new ApiResult { Code = code, Success = true, Message = message, Data = null };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <param name="errorDetail"></param>
        /// <returns></returns>
        public static ApiResult<T> Error<T>(string message, int code = 0, string errorDetail = null)
        {
            return new ApiResult<T> { Success = false, Code = code, Message = message, ErrorDetail = errorDetail, Data = default };
        }

        /// <summary>
        /// 创建失败响应（无数据）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <param name="errorDetail"></param>
        /// <returns></returns>
        public static ApiResult Error(string message, int code = 0, string errorDetail = null)
        {
            return new ApiResult { Success = false, Code = code, Message = message, ErrorDetail = errorDetail, Data = null };
        }
    }
}
