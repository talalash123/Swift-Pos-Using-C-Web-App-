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
        [BindProperty] public string SelectedType { get; set; }
        [BindProperty] public int Stock { get; set; } // Form se ye value aayegi

        public async Task<IActionResult> OnPostAsync()
        {
            // Debugging ke liye: Agar Modelstate invalid hai to check karein
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Naya Product object banana
            var newProduct = new BaseProduct
            {
                Name = Name,
                Category = Category,
                Price = Price,
                // Direct 'Stock' property se value utha kar 'StockQuantity' mein dalna
                StockQuantity = Stock,
                Type = SelectedType ?? "Physical"
            };

            // Double check: Agar SelectedType Service hai to stock 0 kar dein
            if (SelectedType == "Service")
            {
                newProduct.StockQuantity = 0;
            }

            try
            {
                await _context.Products.InsertOneAsync(newProduct);
                // Success! Index page par wapis jayein
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Database Error: " + ex.Message);
                return Page();
            }
        }
    }
}