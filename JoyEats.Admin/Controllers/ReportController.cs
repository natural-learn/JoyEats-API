using JoyEats.Common;
using JoyEats.Core.VO.Report;
using JoyEats.IServices;
using Microsoft.AspNetCore.Mvc;

namespace JoyEats.Admin.Controllers
{
    [Route("admin/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// 营业额数据统计
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet("turnoverStatistics")]
        public async Task<ActionResult<ApiResult<TurnoverReportVO>>> TurnoverStatistics(DateTime beginTime, DateTime endTime)
        {
            TurnoverReportVO turnoverReportDto = await _reportService.GetTurnoverAsync(beginTime, endTime);
            return ApiResultHelper.Success(turnoverReportDto);
        }

        /// <summary>
        /// 用户数据统计
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet("userStatistics")]
        public async Task<ActionResult<ApiResult<UserReportVO>>> UserStatistics(DateTime beginTime, DateTime endTime)
        {
            UserReportVO userReportDto = await _reportService.GetUserStatisticsAsync(beginTime, endTime);
            return ApiResultHelper.Success(userReportDto);
        }
    }
}
