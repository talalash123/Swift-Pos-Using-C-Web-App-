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

        public List<BaseProduct> Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Database se products uthana
            Products = await _context.Products.Find(_ => true).ToListAsync();
        }

        public async Task<IActionResult> OnPostProcessSaleAsync([FromBody] Order orderData)
        {
            if (orderData == null || orderData.Items == null || !orderData.Items.Any())
            {
                return new JsonResult(new { success = false, message = "Cart is empty." });
            }

            try
            {
                orderData.OrderDate = DateTime.Now;

                // 1. Pehle Order save karein
                await _context.Orders.InsertOneAsync(orderData);

                // 2. Har item ki quantity database mein minus karein
                foreach (var item in orderData.Items)
                {
                    var filter = Builders<BaseProduct>.Filter.Eq(p => p.Id, item.ProductId);

                    // Database mein jo value hai usme se item.Quantity minus ho jayegi
                    var update = Builders<BaseProduct>.Update.Inc(p => p.StockQuantity, -item.Quantity);

                    await _context.Products.UpdateOneAsync(filter, update);
                }

                return new JsonResult(new { success = true, message = "Success!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "DB Error: " + ex.Message });
            }
        }
    }
}