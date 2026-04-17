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

        public IndexModel(MongoDbContext context)
        {
            _context = context;
        }

        // List to display products on the terminal
        public List<BaseProduct> Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Database se products uthana taaki terminal pe show hon
            Products = await _context.Products.Find(_ => true).ToListAsync();
        }

        // AJAX handler jo order save karta hai
        public async Task<IActionResult> OnPostProcessSaleAsync([FromBody] Order orderData)
        {
            if (orderData == null)
            {
                return new JsonResult(new { success = false, message = "Empty order data received." });
            }

            try
            {
                // Order details finalize karna
                orderData.OrderDate = DateTime.Now; // Server time set karein

                // MongoDB mein 'Orders' collection mein insert karna
                await _context.Orders.InsertOneAsync(orderData);

                return new JsonResult(new { success = true, message = "Order saved successfully!" });
            }
            catch (Exception ex)
            {
                // Agar koi error aaye to console ya frontend pe return karein
                return new JsonResult(new { success = false, message = "Database Error: " + ex.Message });
            }
        }
    }
}