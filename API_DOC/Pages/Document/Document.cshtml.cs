using API_DOC.Data;
using API_DOC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace API_DOC.Pages.Document ;

    public class DocumentModel : PageModel
    {
        [BindProperty(SupportsGet = true, Name = "InputString")]
        public string InputString { get; set; }
        public int MethodCount { get; set; }
        public UserModel? CurrentUser { get; set; }
        public Method? CurrentMethod { get; set; }
        public List<Method?> MethodList { get; set; }
        public UserDb UserControl{ get; set; }
        public MethodDb MethodControl{ get; set; }
        public readonly AppDbContext Context;
        public List<string> Languages = Statics.Languages;
        public string[] Key = Statics.Key;
        public string[] Value = Statics.Value;

        public DocumentModel(AppDbContext context)
        {
            Context = context;
        }
        
        public void OnGet()
        {
            UserControl =  new UserDb(Context, Request);
            MethodControl =  new MethodDb(Context, Request);
            CurrentUser = UserControl.UserByToken() ?? null;
            if (InputString != "Home" && InputString != "Dictionary")
            {
                CurrentMethod = MethodControl.GetMethod(InputString);
            }
            MethodList = MethodControl.GetMethodList();
            MethodCount = MethodList.Count;
        }

    }