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

        // UI Variables
        public decimal Revenue { get; set; }
        public long TotalOrders { get; set; }
        public int ActiveProductsCount { get; set; }
        public List<Order> RecentTransactions { get; set; } = new();

        public async Task OnGetAsync()
        {
            // 1. Fetch All Orders
            var allOrders = await _context.Orders.Find(_ => true).ToListAsync();

            // 2. Calculate Stats
            Revenue = allOrders.Sum(o => o.GrandTotal);
            TotalOrders = allOrders.Count;

            // 3. Get Recent 5 Transactions
            RecentTransactions = allOrders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToList();

            // 4. Count Products
            var productCount = await _context.Products.CountDocumentsAsync(_ => true);
            ActiveProductsCount = (int)productCount;
        }
    }
}