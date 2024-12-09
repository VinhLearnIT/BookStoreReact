using ApplicationCore.Model.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IStatisticsService
    {
        Task<StatisticsResultModel> GetStatisticsAsync();
        Task<IEnumerable<MonthlyRevenueModel>> GetMonthlyRevenueOfYearAsync(int year);
        Task<IEnumerable<MonthlySalesModel>> GetMonthlySalesOfYearAsync(int year);
    }
}
