using API_DOC.Data;
using API_DOC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API_DOC.Pages.Document ;

    public class Logging : PageModel
    {
        [BindProperty]
        public UserModel Input { get; set; }
        [BindProperty(SupportsGet = true, Name = "State")]
        public string State { get; set; }
        [BindProperty]
        public PassWord MyPass { get; set; }
        public UserModel? CurrentUser { get; set; }
        public UserDb UserControl{ get; set; }
        public List<string> Error { get; set; }
        public readonly AppDbContext Context;
        
        public Logging(AppDbContext dbContext)
        {
            Context = dbContext;
        }

        public void OnGet()
        {
            if (TempData.TryGetValue("Error", out var list))
            {
                if (list is string[] errs)
                {
                    Error = new List<string>();
                    foreach (var VARIABLE in errs)
                    {
                        Error.Add(VARIABLE);
                    }
                    TempData["Error"] = null;
                }
            }
            UserControl =  new UserDb(Context, Request);
            CurrentUser = UserControl.UserByToken() ?? null;
            if (State.Equals("LogOut"))
            {
                CurrentUser = null;
                Response.Cookies.Delete("AuthToken");
            }
        }
        
         
        public IActionResult OnPost()
        {
            UserControl =  new UserDb(Context, Request);
            if (ModelState.IsValid)
            {
                if (State.Equals("LogIn"))
                {
                    UserModel? user = UserControl.UserByLogin(Input);
                    if (user is {Acceptance: true })
                    {
                        var token = Statics.GenerateAuthToken(user);
                        user.Token = token;
                        Context.SaveChanges();

                        Statics.SetAuthCookie(token, Response);
                    
                        return Redirect("/Document");
                    }
                    List<string> result = new List<string>();
                    if (user == null)
                    {
                        result.Add("نام کاربری یا رمز عبور اشتباه است");
                    }else if (!user.Acceptance)
                    {
                        result.Add("اکانت شما در حال بررسی برای تایید است");
                    }
                    TempData["Error"] = result ;
                    return Redirect("/Logging/LogIn");
                }
                else if (State.Equals("Register"))
                {
                    (List<bool> errorBool, List<string> result) = Statics.RegisterConditions(UserControl, Input, MyPass);
                    if (!errorBool.Contains(false))
                    {
                        UserControl.AddUser(Input, MyPass);
                        return Redirect("/Logging/LogIn");
                    }
                    TempData["Error"] = result ;
                    return Redirect("/Logging/Register");
                }
            }
            return Redirect("/none");
            
        }
    }