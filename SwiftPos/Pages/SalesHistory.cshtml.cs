using Microsoft.AspNetCore.Mvc;
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

        // Is property ka hona zaroori hai search ke liye
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public async Task OnGetAsync()
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                // Regex search jo name aur ID dono ko check karegi (Case-Insensitive)
                filter = filterBuilder.Or(
                    filterBuilder.Regex(o => o.CustomerName, new MongoDB.Bson.BsonRegularExpression(SearchTerm, "i")),
                    filterBuilder.Regex(o => o.Id, new MongoDB.Bson.BsonRegularExpression(SearchTerm, "i"))
                );
            }

            Orders = await _context.Orders
                .Find(filter)
                .SortByDescending(o => o.OrderDate)
                .ToListAsync();

            TotalPeriodRevenue = Orders.Sum(o => o.GrandTotal);
        }
    }
}