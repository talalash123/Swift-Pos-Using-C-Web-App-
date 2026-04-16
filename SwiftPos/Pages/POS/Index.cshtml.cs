using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using SwiftPOS.Data;
using SwiftPOS.Models;

namespace SwiftPOS.Pages.POS
{
    public class IndexModel : PageModel
    {
        private readonly MongoDbContext _context;
        public IndexModel(MongoDbContext context) => _context = context;
        public List<BaseProduct> Products { get; set; } = new();

        public async Task OnGetAsync() => Products = await _context.Products.Find(_ => true).ToListAsync();

        public async Task<IActionResult> OnPostCheckoutAsync([FromBody] Order order)
        {
            if (order == null || order.Items.Count == 0) return BadRequest();
            await _context.Orders.InsertOneAsync(order);
            // Inventory update logic can be added here
            return new JsonResult(new { success = true });
        }
    }
}