using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using SwiftPOS.Data;
using SwiftPOS.Models;

namespace SwiftPOS.Pages.Inventory
{
    public class IndexModel : PageModel
    {
        private readonly MongoDbContext _context;
        public IndexModel(MongoDbContext context) => _context = context;
        public List<BaseProduct> Products { get; set; } = new();
        public async Task OnGetAsync() => Products = await _context.Products.Find(_ => true).ToListAsync();
    }
}