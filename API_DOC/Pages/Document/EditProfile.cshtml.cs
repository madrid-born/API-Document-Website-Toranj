using System.ComponentModel.DataAnnotations;
using API_DOC.Data;
using API_DOC.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static API_DOC.Pages.Document.Logging;
namespace API_DOC.Pages.Document ;

    public class EditProfile : PageModel
    {
        [BindProperty(SupportsGet = true, Name = "BrandInput")]
        public string BrandInput { get; set; } = "";
        [BindProperty(SupportsGet = true, Name = "State")]
        public string State { get; set; }
        [BindProperty]
        public UserModel Input { get; set; }

        [BindProperty]
        public PassWord MyPass { get; set; }
        public UserModel? CurrentUser { get; set; }
        public UserDb UserControl{ get; set; }
        public List<string> Error { get; set; }
        public readonly AppDbContext Context;
        
        public EditProfile(AppDbContext context)
        {
            Context = context;
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
        }
        
        public IActionResult OnPost()
        {
            UserControl = new UserDb(Context, Request);
            CurrentUser = UserControl.UserByBrandName(BrandInput) ?? null;
            if (ModelState.IsValid)
            {
                if (State.Equals("EditPassword"))
                {
                    if (Statics.PassConditions(MyPass,CurrentUser.Password ))
                    {
                        UserControl.ChangePassword(CurrentUser.Username, MyPass.Current);
                        return Redirect("/Logging/LogIn");
                    }
                    List<string> result = new List<string>();
                    result.Add("تطابق رمز ها یا رمز قبلی دارای ایراد است");
                    TempData["Error"] = result ;
                    return Redirect($"/{BrandInput}/EditPassword");
                }
                else
                {
                    (List<bool> errorBool, List<string> result) = Statics.ProfileConditions(UserControl, Input, CurrentUser.Username);
                    if (!errorBool.Contains(false))
                    {
                        UserControl.ChangeProfile(CurrentUser.Username, Input);
                        return Redirect("/Logging/LogIn");
                    }
                    TempData["Error"] = result ;
                    return Redirect($"/{BrandInput}/EditProfile");
                }
            }
            return Redirect("/none");
        }
    }