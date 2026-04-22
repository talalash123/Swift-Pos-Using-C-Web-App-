using Microsoft.AspNetCore.Mvc;
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

        public int TotalOrders { get; set; }
        public decimal Revenue { get; set; }
        public int ActiveProductsCount { get; set; }
        public List<Order> RecentTransactions { get; set; } = new();
        public List<string> ChartLabels { get; set; } = new();
        public List<decimal> ChartData { get; set; } = new();

        public async Task OnGetAsync()
        {
            var allOrders = await _context.Orders.Find(_ => true).ToListAsync();
            var allProducts = await _context.Products.Find(_ => true).ToListAsync();

            TotalOrders = allOrders.Count;
            Revenue = allOrders.Sum(o => o.GrandTotal);
            ActiveProductsCount = allProducts.Count;

            RecentTransactions = allOrders.OrderByDescending(o => o.OrderDate).Take(5).ToList();

            // Weekly Chart Logic
            for (int i = 6; i >= 0; i--)
            {
                var targetDate = DateTime.Now.AddDays(-i).Date;
                ChartLabels.Add(targetDate.ToString("ddd").ToUpper());
                var dayTotal = allOrders
                    .Where(o => o.OrderDate.ToLocalTime().Date == targetDate)
                    .Sum(o => o.GrandTotal);
                ChartData.Add(dayTotal);
            }
        }

        // --- REAL-TIME ENDPOINT: STOCK DATA ---
        public async Task<IActionResult> OnGetStockDataAsync()
        {
            var allProducts = await _context.Products.Find(_ => true).ToListAsync();

            var inStock = allProducts.Count(p => p.StockQuantity > 10);
            var lowStock = allProducts.Count(p => p.StockQuantity <= 10 && p.StockQuantity > 0);
            var outOfStock = allProducts.Count(p => p.StockQuantity <= 0);

            return new JsonResult(new[] { inStock, lowStock, outOfStock });
        }

        // --- REAL-TIME ENDPOINT: RECENT TRANSACTIONS ---
        public async Task<IActionResult> OnGetRecentTransactionsAsync()
        {
            var latestOrders = await _context.Orders.Find(_ => true)
                .SortByDescending(o => o.OrderDate)
                .Limit(5)
                .ToListAsync();

            // Formatting for JSON
            var result = latestOrders.Select(o => new {
                id = o.Id.Length > 8 ? o.Id.Substring(o.Id.Length - 8) : o.Id,
                customerName = o.CustomerName,
                total = o.GrandTotal.ToString("N2"),
                time = o.OrderDate.ToLocalTime().ToString("hh:mm tt")
            });

            return new JsonResult(result);
        }
    }
}