using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SwiftPOS.Pages.Settings
{
    public class SettingsModel : PageModel
    {
        // No Database code here - Safe from errors
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Just refresh for demo
            return RedirectToPage();
        }
    }
}