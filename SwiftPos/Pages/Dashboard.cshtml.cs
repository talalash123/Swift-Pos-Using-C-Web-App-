using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using SwiftPOS.Data;
using SwiftPOS.Models;

namespace SwiftPOS.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly MongoDbContext _context;

        public DashboardModel(MongoDbContext context)
        {
            _context = context;
        }

        // Stats Properties
        public int TotalOrders { get; set; }
        public decimal Revenue { get; set; }
        public int ActiveProductsCount { get; set; }

        // Error fix: Ab yahan 'Order' model use ho raha hai
        public List<Order> RecentTransactions { get; set; } = new();

        // Chart Properties
        public List<string> ChartLabels { get; set; } = new();
        public List<decimal> ChartData { get; set; } = new();

        public async Task OnGetAsync()
        {
            // 1. Database se sara data fetch karein
            var allOrders = await _context.Orders.Find(_ => true).ToListAsync();
            var allProducts = await _context.Products.Find(_ => true).ToListAsync();

            // 2. Summary Stats calculate karein
            TotalOrders = allOrders.Count;
            Revenue = allOrders.Sum(o => o.GrandTotal);
            ActiveProductsCount = allProducts.Count;

            // 3. Recent 5 Transactions (Latest first)
            RecentTransactions = allOrders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToList();

            // 4. Real-time Chart Logic (Last 7 Days)
            ChartLabels.Clear();
            ChartData.Clear();

            for (int i = 6; i >= 0; i--)
            {
                var targetDate = DateTime.Now.AddDays(-i).Date;

                // Day Label (e.g., MON, TUE)
                ChartLabels.Add(targetDate.ToString("ddd").ToUpper());

                // Calculate total for this specific day (Local Time fix)
                var dayTotal = allOrders
                    .Where(o => o.OrderDate.ToLocalTime().Date == targetDate)
                    .Sum(o => o.GrandTotal);

                ChartData.Add(dayTotal);
            }
        }
    }
}