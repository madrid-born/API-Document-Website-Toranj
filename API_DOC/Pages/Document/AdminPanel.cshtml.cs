using API_DOC.Data;
using API_DOC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API_DOC.Pages.Document ;

    public class AdminPanel : PageModel
    {
        [BindProperty(SupportsGet = true, Name = "State")]
        public string State { get; set; } = "";
        public List<UserModel?> UserList { get; set; }
        public Method SelectedMethod { get; set; }
        public int MethodCount { get; set; }
        public List<Method?> MethodList { get; set; }
        public UserModel? CurrentUser { get; set; }
        public MethodDb MethodControl{ get; set; }
        public UserDb UserControl{ get; set; }
        public readonly AppDbContext Context;
        
        public AdminPanel(AppDbContext context)
        {
            Context = context;
        }
        
        public void OnGet()
        {
            MethodControl = new MethodDb(Context, Request);
            UserControl =  new UserDb(Context, Request);
            MethodList = MethodControl.GetMethodList();
            MethodCount = MethodList.Count;
            UserList = UserControl.NotAccepted();
            CurrentUser = UserControl.UserByToken() ?? null;
            
        }

        public void OnPost()
        {
            UserControl =  new UserDb(Context, Request);
            CurrentUser = UserControl.UserByToken() ?? null;
            UserList = UserControl.NotAccepted();
        }
        
        public IActionResult OnPostAccept(string userName)
        {
            UserControl =  new UserDb(Context, Request);
            UserControl.AcceptUser(userName);
            CurrentUser = UserControl.UserByToken() ?? null;
            return Redirect("/Admin/Panel");
        }

        public IActionResult OnPostReject(string userName)
        {
            UserControl =  new UserDb(Context, Request);
            UserControl.RejectUser(userName);
            CurrentUser = UserControl.UserByToken() ?? null;
            return Redirect("/Admin/Panel");
        }
        
        public IActionResult OnPostSelect(string selectedMethod)
        {
            MethodControl = new MethodDb(Context, Request);
            SelectedMethod = MethodControl.GetMethod(selectedMethod);
            return Redirect("/AdminPanel/Panel");
        }
    }