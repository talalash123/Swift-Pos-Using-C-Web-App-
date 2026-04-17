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

        // List ka naam 'Orders' rakha hai taaki frontend loop error na de
        public List<Order> Orders { get; set; } = new();
        public decimal TotalPeriodRevenue { get; set; }

        public async Task OnGetAsync()
        {
            // Database se saare orders fetch karna aur latest ko top par rakhna
            Orders = await _context.Orders
                .Find(_ => true)
                .SortByDescending(o => o.OrderDate)
                .ToListAsync();

            // Total revenue calculate karna overview card ke liye
            if (Orders != null)
            {
                TotalPeriodRevenue = Orders.Sum(o => o.GrandTotal);
            }
        }
    }
}