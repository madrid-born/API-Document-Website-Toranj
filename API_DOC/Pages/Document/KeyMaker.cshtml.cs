using API_DOC.Data;
using API_DOC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API_DOC.Pages.Document ;

    public class KeyMaker : PageModel
    {
        [BindProperty(SupportsGet = true, Name = "BrandInput")]
        public string BrandInput { get; set; } = "";
        [BindProperty(SupportsGet = true, Name = "MethodInput")]
        public string MethodInput { get; set; } = "";
        [BindProperty]
        public Dictionary<string, string> Dict { get; set; } = Statics.Dictionary;
        public UserModel? CurrentUser { get; set; }
        public UserDb UserControl{ get; set; }
        public MethodDb MethodControl{ get; set; }
        public int MethodCount { get; set; }
        public Method? CurrentMethod { get; set; }
        public List<Method?> MethodList { get; set; }
        public List<string> Languages = Statics.Languages;
        public readonly AppDbContext Context;
        
        public KeyMaker(AppDbContext context)
        {
            Context = context;
        }

        public void OnGet()
        {
            UserControl =  new UserDb(Context, Request);
            MethodControl =  new MethodDb(Context, Request);
            CurrentUser = UserControl.UserByToken() ?? null;
            if (MethodInput != "Home" && MethodInput != "Dictionary")
            {
                CurrentMethod = MethodControl.GetMethod(MethodInput);
            }
            MethodList = MethodControl.GetMethodList();
            MethodCount = MethodList.Count;
            Dict = GetDict();
        }

        public Dictionary<string, string> GetDict()
        {
            Dictionary<string, string> res = Statics.Dictionary;
            if (TempData.TryGetValue("Dictionary", out var dict))
            {
                if (dict is Dictionary<string, string> receivedDict)
                {
                    res =  receivedDict;
                }
            }
            return res;
        }

        public IActionResult OnPost()
        {
            UserControl =  new UserDb(Context, Request);
            MethodControl =  new MethodDb(Context, Request);
            CurrentUser = UserControl.UserByToken() ?? null;
            if (MethodInput != "Home" && MethodInput != "Dictionary")
            {
                CurrentMethod = MethodControl.GetMethod(MethodInput);
            }
            if (ModelState.IsValid)
            {
                TempData["Dictionary"] = Dict;
                return Redirect($"{CurrentUser.BrandName}");
            }
            return Redirect("/Document");
        }
        
        public List<string> ElementFinder()
        {
            List<string> list = new List<string>();
            foreach (var key in Dict.Keys)
            {
                if (CurrentMethod.Key.Contains($"<{key}>"))
                {
                    list.Add(key);
                }
            }
            return list;
        }
    }
    

