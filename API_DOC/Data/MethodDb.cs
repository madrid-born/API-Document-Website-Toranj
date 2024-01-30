using API_DOC.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_DOC.Data ;

    public class MethodDb
    {
        private HttpRequest Request { get; set; }
        private readonly AppDbContext _context;
        
        public MethodDb(AppDbContext context, HttpRequest request)
        {
            _context = context;
            Request = request;
            GetMethodList();
        }
        
        public List<Method?> GetMethodList()
        {
            return  _context.Methods.Where(method => method.Id > 0).ToList();
        }
        
        public Method? GetMethod(string methodName)
        {
            return _context.Methods.FirstOrDefault(method => method.Name == methodName);
        }
        
        public void ChangeData(string methodName, Method input)
        {
            Method? method = GetMethod(methodName);
            if (method != null)
            {
                method.Name = input.Name;
                method.PersianName = input.PersianName;
                method.Details = input.Details;
                method.Key = input.Key;
                method.CsCode = input.CsCode;
                method.Php1Code = input.Php1Code;
                method.Php2Code = input.Php2Code;
                method.PythonCode = input.PythonCode;
                method.NodeJsCode = input.NodeJsCode;
                method.OutPut = input.OutPut;

            }
            _context.SaveChanges();
        }

        public void AddData(Method input)
        {

            var method = new Method
            {
                Name = input.Name,
                PersianName = input.PersianName,
                Details = input.Details,
                Key = input.Key,
                CsCode = input.CsCode,
                Php1Code = input.Php1Code,
                Php2Code = input.Php2Code,
                PythonCode = input.PythonCode,
                NodeJsCode = input.NodeJsCode,
                OutPut = input.OutPut
            };
            _context.Methods.Add(method);
            _context.SaveChanges();
        }
    }