using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SwiftPOS.Data;
using SwiftPOS.Models;

namespace SwiftPOS.Pages.Inventory
{
    public class CreateModel : PageModel
    {
        private readonly MongoDbContext _context;
        public CreateModel(MongoDbContext context) { _context = context; }

        [BindProperty] public string Name { get; set; }
        [BindProperty] public string Category { get; set; }
        [BindProperty] public decimal Price { get; set; }

        // YE PROPERTIES LAAZMI HAIN (Error Fix)
        [BindProperty] public string SelectedType { get; set; }
        [BindProperty] public int Stock { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var newProduct = new BaseProduct
            {
                Name = Name,
                Category = Category,
                Price = Price
            };

            await _context.Products.InsertOneAsync(newProduct);
            return RedirectToPage("./Index");
        }
    }
}