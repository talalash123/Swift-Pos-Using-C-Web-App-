using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using SwiftPOS.Data;
using SwiftPOS.Models;

namespace SwiftPOS.Pages.Inventory
{
    public class IndexModel : PageModel
    {
        private readonly MongoDbContext _context;

        public IndexModel(MongoDbContext context)
        {
            _context = context;
        }

        public List<BaseProduct> Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Database se products fetch karna
            Products = await _context.Products.Find(_ => true).ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return Page();

            var filter = Builders<BaseProduct>.Filter.Eq(p => p.Id, id);
            await _context.Products.DeleteOneAsync(filter);

            return RedirectToPage();
        }
    }
}