using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using SwiftPOS.Data;
using SwiftPOS.Models;

namespace SwiftPOS.Pages
{
    public class SalesHistoryModel : PageModel
    {
        private readonly MongoDbContext _context;

        public SalesHistoryModel(MongoDbContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; } = new();
        public decimal TotalPeriodRevenue { get; set; }

        public async Task OnGetAsync()
        {
            // Sare orders fetch karein aur latest orders ko sabse upar rakhein
            Orders = await _context.Orders
                .Find(_ => true)
                .SortByDescending(o => o.OrderDate)
                .ToListAsync();

            // Summary card ke liye total revenue calculate karein
            TotalPeriodRevenue = Orders.Sum(o => o.GrandTotal);
        }
    }
}