using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using SwiftPOS.Data;
using SwiftPOS.Models;

namespace SwiftPOS.Pages.Inventory
{
    public class EditModel : PageModel
    {
        private readonly MongoDbContext _context;
        public EditModel(MongoDbContext context) { _context = context; }

        [BindProperty]
        public BaseProduct Product { get; set; }

        // 1. Load data when page opens
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToPage("./Index");

            Product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (Product == null) return RedirectToPage("./Index");

            return Page();
        }

        // 2. Save updated data
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var filter = Builders<BaseProduct>.Filter.Eq(p => p.Id, Product.Id);
            var update = Builders<BaseProduct>.Update
                .Set(p => p.Name, Product.Name)
                .Set(p => p.Category, Product.Category)
                .Set(p => p.Price, Product.Price);

            await _context.Products.UpdateOneAsync(filter, update);

            return RedirectToPage("./Index");
        }
    }
}