using SkyTakeOut.Common;

namespace SkyTakeOut.Admin.Middlewares
{
    public class GlobalExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        private readonly RequestDelegate _next = next;
        private readonly IWebHostEnvironment _env = env;

        /// <summary>
        /// 捕获后续中间件的异常并处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // 没有异常则执行下一个中间件
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // 设置响应内容类型为JSON
            context.Response.ContentType = "application/json";

            // 根据异常类型构建统一响应
            var response = ex switch
            {
                // 自定义异常类型
                BusinessException businessEx => ApiResultHelper.Error(
                    businessEx.Message, businessEx.Code,
                    _env.IsDevelopment() ? businessEx.StackTrace : null),

                ArgumentException argEx => ApiResultHelper.Error(
                    "参数错误：" + argEx.Message,
                    400,
                    _env.IsDevelopment() ? argEx.StackTrace : null),

                KeyNotFoundException keyEx => ApiResultHelper.Error(
                    "资源不存在: " + keyEx.Message,
                    404,
                    _env.IsDevelopment() ? ex.StackTrace : null),

                // 其他未处理异常
                _ => ApiResultHelper.Error(
                    _env.IsDevelopment() ? ex.Message : "服务器内部错误，请稍后重试",
                    500,
                    _env.IsDevelopment() ? ex.StackTrace : null)
            };

            context.Response.StatusCode = response.Code;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
