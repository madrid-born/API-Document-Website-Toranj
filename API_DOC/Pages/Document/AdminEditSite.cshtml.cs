using API_DOC.Data;
using API_DOC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API_DOC.Pages.Document ;

    public class AdminEditSite : PageModel
    {
        [BindProperty(SupportsGet = true, Name = "InputMethod")]
        public string InputMethod { get; set; }
        [BindProperty]
        public Method Input { get; set; }
        public Method CurrentMethod { get; set; }
        public int MethodCount { get; set; }
        public List<Method?> MethodList { get; set; }
        public UserModel? CurrentUser { get; set; }
        public MethodDb MethodControl{ get; set; }
        public UserDb UserControl{ get; set; }
        public readonly AppDbContext Context;
        
        public AdminEditSite(AppDbContext context)
        {
            Context = context;
        }
        
        public void OnGet()
        {
            MethodControl = new MethodDb(Context, Request);
            UserControl =  new UserDb(Context, Request);
            MethodList = MethodControl.GetMethodList();
            MethodCount = MethodList.Count;
            CurrentUser = UserControl.UserByToken() ?? null;
            if (InputMethod != "Home" && InputMethod != "Dictionary")
            {
                CurrentMethod = MethodControl.GetMethod(InputMethod);
            }
            MethodList = MethodControl.GetMethodList();
            MethodCount = MethodList.Count;
        }
        
        public IActionResult OnPostEdit()
        {
            MethodControl = new MethodDb(Context, Request);
            if (ModelState.IsValid)
            {
                MethodControl.ChangeData(InputMethod, Input);
                return Redirect($"/Document/{Input.Name}");
            }

            // Model is not valid, redisplay the page with validation errors
            return Redirect("/Document");
        }
        
        public IActionResult OnPostNew()
        {
            MethodControl = new MethodDb(Context, Request);
            if (ModelState.IsValid)
            {
                MethodControl.AddData(Input);
                return Redirect($"/Document/{Input.Name}");
            }

            // Model is not valid, redisplay the page with validation errors
            return Redirect("/Document");
        }
    }