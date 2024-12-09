using ApplicationCore.Interfaces;
using ApplicationCore.Model.Statistics;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext _context;

        public StatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<StatisticsResultModel> GetStatisticsAsync()
        {
            var currentDate = DateTime.Now;
            int currentMonth = currentDate.Month;
            int currentYear = currentDate.Year;

            var totalOrders = await _context.Orders
                .Where(o => o.OrderDate.Month == currentMonth && o.OrderDate.Year == currentYear && o.OrderStatus != "Canceled")
                .CountAsync();

            var pendingOrders = await _context.Orders
                .Where(o => o.OrderStatus == "Pending")
                .CountAsync();

            var totalRevenue = await _context.OrderDetails
                .Where(od => od.Order.OrderDate.Month == currentMonth && od.Order.OrderDate.Year == currentYear && od.Order.OrderStatus != "Canceled")
                .SumAsync(od => od.Quantity * od.Price);

            var totalUsers = await _context.Customers.CountAsync();

            var topSellingBooks = await _context.OrderDetails
                .Where(od => od.Order.OrderStatus != "Canceled")
                .GroupBy(od => od.BookID) 
                .Select(g => new TopSellingBookModel
                {
                    BookID = g.Key,
                    BookName = g.First().Book.BookName,
                    ImagePath = g.First().Book.ImagePath,
                    TotalQuantitySold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(b => b.TotalQuantitySold)
                .Take(3)
                .ToListAsync();

            return new StatisticsResultModel
            {
                TotalOrders = totalOrders,
                PendingOrders = pendingOrders,
                TotalRevenue = totalRevenue,
                TotalUsers = totalUsers,
                TopSellingBooks = topSellingBooks
            };
        }
        public async Task<IEnumerable<MonthlyRevenueModel>> GetMonthlyRevenueOfYearAsync(int year)
        {
            try
            {
                var result = await _context.OrderDetails
                .Where(od => od.Order.OrderDate.Year == year && od.Order.OrderStatus != "Cancelled" && od.Order.OrderStatus != "Pendding")
                .GroupBy(od => od.Order.OrderDate.Month)
                .Select(g => new MonthlyRevenueModel
                {
                    Month = g.Key,
                    TotalRevenue = g.Sum(od => od.Quantity * od.Price)
                })
                .OrderBy(r => r.Month)
                .ToListAsync();

                var allMonths = Enumerable.Range(1, 12);

                var finalResult = allMonths.Select(month =>
                {
                    var monthData = result.FirstOrDefault(r => r.Month == month);
                    return new MonthlyRevenueModel
                    {
                        Month = month,
                        TotalRevenue = monthData?.TotalRevenue ?? 0
                    };
                }).ToList();

                return finalResult;
            }
            catch (Exception ex)
            {

                throw new Exception("Đã có lỗi xảy ra " + ex.Message, ex);
            }

        }

        public async Task<IEnumerable<MonthlySalesModel>> GetMonthlySalesOfYearAsync(int year)
        {
            try
            {
                var result = await _context.OrderDetails
                .Where(od => od.Order.OrderDate.Year == year && od.Order.OrderStatus != "Cancelled" && od.Order.OrderStatus != "Pendding")
                .GroupBy(od => od.Order.OrderDate.Month)
                .Select(g => new MonthlySalesModel
                {
                    Month = g.Key,
                    TotalSales = g.Sum(od => od.Quantity)
                })
                .OrderBy(s => s.Month)
                .ToListAsync();

                var allMonths = Enumerable.Range(1, 12);

                var finalResult = allMonths.Select(month =>
                {
                    var monthData = result.FirstOrDefault(r => r.Month == month);
                    return new MonthlySalesModel
                    {
                        Month = month,
                        TotalSales = monthData?.TotalSales ?? 0
                    };
                }).ToList();

                return finalResult;
            }
            catch (Exception ex)
            {

                throw new Exception("Đã có lỗi xảy ra " + ex.Message, ex);
            }

        }

    }
}
