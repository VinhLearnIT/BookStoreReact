using ApplicationCore.Interfaces;
using ApplicationCore.Model.Auth;
using ApplicationCore.Model.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Admin, Manager")]
    public class StatisticsController : Controller
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        [HttpGet]
        public async Task<ActionResult<StatisticsResultModel>> GetMonthlyStatistics()
        {
            var statistics = await _statisticsService.GetStatisticsAsync();
            return Ok(statistics);
        }

        [HttpGet("{year}")]
        public async Task<IActionResult> GetMonthlyRevenueOfYear(int year)
        {
            var data = await _statisticsService.GetMonthlyRevenueOfYearAsync(year);
            return Ok(data);
        }

        [HttpGet("{year}")]
        public async Task<IActionResult> GetMonthlySalesOfYear(int year)
        {
            var data = await _statisticsService.GetMonthlySalesOfYearAsync(year);
            return Ok(data);
        }
    }
}
