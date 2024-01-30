using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API_DOC.Pages ;

    public class Index : PageModel
    {
        public ActionResult OnGet()
        {
            return Redirect("/Document/Home");
        }
    }