using API_DOC.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_DOC.Data ;

    public class UserDb
    {
        private HttpRequest Request { get; set; }
        private readonly AppDbContext _context;
        public UserDb(AppDbContext context, HttpRequest request)
        {
            _context = context;
            Request = request;
        }
        public UserModel? UserByToken()
        {
            // get the user by token and return it
            Request.Cookies.TryGetValue("AuthToken", out var token);
            return _context.Users.FirstOrDefault(u => u.Token == token);
        }

        public UserModel? UserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
       
        public UserModel? UserByBrandName(string brandName)
        {
            return _context.Users.FirstOrDefault(u => u.BrandName == brandName);
        }
        
        public UserModel? UserByLogin(UserModel Input)
        {
            return _context.Users.FirstOrDefault(u => u.Username == Input.Username && u.Password == Input.Password);        }

        public bool CheckUserName(string userName)
        {
            List<string> UserNames= _context.Users.Where(u => u.Id > 0).Select(obj => obj.Username).ToList();
            return !UserNames.Contains(userName);
        }
        public bool CheckUserName(string userName, string currentUserName)
        {
            if (userName.Equals(currentUserName)) 
                return true;
            List<string> UserNames= _context.Users.Where(u => u.Id > 0).Select(obj => obj.Username).ToList();
            return !UserNames.Contains(userName);
        }
        public void AcceptUser(string userName)
        {
            UserModel? user = UserByUsername(userName);
            if (user != null) user.Acceptance = true;
            _context.SaveChanges();
        }

        public void RejectUser(string userName)
        {
            UserModel? user = UserByUsername(userName);
            if (user != null) _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public void AddUser(UserModel input, PassWord myPass)
        {
            var newUser = new UserModel
            {
                Username = input.Username,
                Password = myPass.Current,
                Email = input.Email,
                BrandName = input.BrandName, 
                Phone = input.Phone
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public void ChangePassword(string username, string newPassword)
        {
            UserModel? user = UserByUsername(username);
            if (user != null) user.Password = newPassword;
            _context.SaveChanges();
        }
        
        public void ChangeProfile(string username, UserModel input)
        {
            var user = UserByUsername(username);
            if (user != null)
            {
                user.Email = input.Email;
                user.Phone = input.Phone;
                user.BrandName = input.BrandName;
                user.Username = input.Username;
            }
            _context.SaveChanges();
        }
        
        public List<UserModel?> NotAccepted()
        {
            return _context.Users.Where(u => u.Acceptance == false).ToList();
        }
    }