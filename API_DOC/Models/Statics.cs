using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using API_DOC.Data;
using API_DOC.Pages.Document;
using NuGet.Protocol.Core.Types;

namespace API_DOC.Models ;

    public static class Statics
    {
        public static List<string> Languages = new()
        {
            "C# - HttpClient", "PHP – cURL", "PHP – HTTP-Request2", "Python – Requests", "NodeJS – Native"
        };

        public static Dictionary<string, string> Dictionary = new Dictionary<string, string>()
        {
            {"BrandName", ""}, {"Mobile", ""}, {"CustomerID", ""},{"ID", ""}, {"AddressTitle", ""},{"AddressLocation", ""},
            {"AdditionAddress", ""},{"Name", ""}, {"FamilyName", ""},{"BirthDate", ""},{"Sexuality", ""},
            {"Address", ""}, {"KBarcode", ""},{"BarcodeMain_ID", ""}, {"CouponSerial", ""},{"GCNum", ""},
            {"GCBalance", ""}
        };
        public static string[] Key =
        {
            "Root", "BrandName", "ReportKey", "Mobile", "CustomerID", "KBarcode", "BarcodeMain_ID",
            "CouponSerial", "GCNum", "GCBalance", "Name", "FamilyName", "BirthDate", "Sexuality", "Address"
        };
        public static string[] Value =
        {
            "ریشه ، کلید مورد نیاز برای فراخوانی هر کلید ",
            "نام شرکت ، برای هر درخواست شما باید نام شرکتتان ذکر شود",
            "عنوان متد ارسالی",
            "شماره موبایل",
            "آیدی مشتری ثبت شده در سیستم شما",
            "بارکد فیزیکی کالای ثبت شده در سیستم شما",
            "آیدی بارکد ثبت شده در سیستم شما",
            "سریال کد تخفیف",
            "شماره کارت هدیه",
            "اعتبار کارت هدیه",
            "نام",
            "نام خانودگی ",
            "تاریخ تولد ",
            "جنسیت که 1 نشان دهنده مرد و 0 نشان دهنده زن بودن است",
            "آدرس "
        };


        static string[] GetElementNames(string xmlInput)
        {
            List<string> elementNames = new List<string>();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlInput);

                XmlNode root = xmlDoc.DocumentElement;
                foreach (XmlNode node in root.ChildNodes)
                {
                    elementNames.Add(node.Name);
                }
            }
            catch (XmlException ex)
            {
                Console.WriteLine("Error parsing XML: " + ex.Message);
            }

            return elementNames.ToArray();
        }
        
        public static bool IsUserName(string input)
        {
            string pattern = @"^[a-zA-Z_\-]+$";
            return Regex.IsMatch(input, pattern);
        }
        public static bool IsEmail(string input)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            return Regex.IsMatch(input, pattern);
        }
        
        public static bool IsBrandName(string input)
        {
            string pattern = @"^[a-zA-Z]+$";
            return Regex.IsMatch(input, pattern);
        }

        public static bool IsPhoneNumber(string input)
        {
            if (input.Length != 11 || !input.StartsWith("09"))
            {
                return false;
            }
            for (int i = 2; i < input.Length; i++)
            {
                if (!char.IsDigit(input[i]))
                {
                    return false;
                }
            }
            return true;
        }
        
        public static (List<bool>, List<string>) RegisterConditions(UserDb userControl, UserModel? user, PassWord MyPass)
        {
            List<bool> errs = new List<bool>()
            {
                userControl.CheckUserName(user.Username),
                IsEmail(user.Email) ,
                IsUserName(user.Username) , 
                IsPhoneNumber(user.Phone) , 
                IsBrandName(user.BrandName) , 
                MyPass.Current.Equals(MyPass.Confirm)
            };
            List<string> errDetails = new List<string>()
            {
                "نام کاربری موجود نیست",
                "ایمیل دارای فرمت صحیح نیست" ,
                "نام کاربری دارای فرمت صحیح نیست",
                "شماره تلفن دارای فرمت صحیح نیست",
                "نام شرکت دارای فرمت صحیح نیست",
                "رمز عبور با تایید خود مطابقت ندارد"

            };
            List<string> result = new List<string>();
            for (int i = 0; i < 6; i++)
            {
                if (!errs[i])
                {
                   result.Add(errDetails[i]);
                }
            }
            return (errs, result);
        }
        
        public static (List<bool>, List<string>) ProfileConditions(UserDb userControl,UserModel input, string userName)
        {
            List<bool> errs = new List<bool>()
            {
                userControl.CheckUserName(input.Username, userName),
                IsEmail(input.Email) ,
                IsUserName(input.Username) , 
                IsPhoneNumber(input.Phone) , 
                IsBrandName(input.BrandName) , 
            };
            List<string> errDetails = new List<string>()
            {
                "نام کاربری موجود نیست",
                "ایمیل دارای فرمت صحیح نیست" ,
                "نام کاربری دارای فرمت صحیح نیست",
                "شماره تلفن دارای فرمت صحیح نیست",
                "نام شرکت دارای فرمت صحیح نیست"

            };
            List<string> result = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                if (!errs[i])
                {
                    result.Add(errDetails[i]);
                }
            }
            return (errs, result);
        }
        
        public static bool PassConditions(PassWord myPass, string currentPassword)
        {
            if (myPass.Past == currentPassword && myPass.Confirm == myPass.Current)
                return true;
            return false;
        }
        
        public static string GenerateAuthToken(UserModel? user)
        {
            string token = user.Username + user.Password + DateTime.Now.Date;
        
            // Convert the token string to bytes
            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
        
            // Create an instance of SHA256
            using SHA256 sha256 = SHA256.Create();
            // Compute the hash value
            byte[] hashBytes = sha256.ComputeHash(tokenBytes);

            // Convert the hash bytes to a hexadecimal string
            var generatedToken = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return generatedToken;
        }

        
        public static void SetAuthCookie(string token, HttpResponse response)
        {
            response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(8), // Adjust the expiration as needed
                HttpOnly = true, // Prevent client-side JavaScript access
                Secure = true, // Send only over HTTPS in production
                SameSite = SameSiteMode.Strict // Prevent cross-site request forgery
            });
        }

        public static string Revised(Dictionary<string, string> dict,int section, string text)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"BrandName", "DadehFa"},{"Mobile", "09123456789"}, {"CustomerID", "1234"},{"ID", "123"}, {"AddressTitle", "title"},
                {"AddressLocation", "location"},{"AdditionAddress", "add-ons"},{"Name", "Ali"},
                {"FamilyName", "Ahmadi"},{"BirthDate", "2001-01-01"},{"Sexuality", "1"},{"Address", "Mashhad"},
                {"KBarcode", "000012340000"},{"BarcodeMain_ID", "1234"}, {"CouponSerial", "1234"},{"GCNum", "1234"},
                {"GCBalance", "1234"}
            };
            bool expression = section == 1;
            string result = text;
            foreach (var key in dict.Keys)
            {
                result = result.Replace(Example(key, dictionary[key], expression), Example(key, dict[key], expression));
            }
            return result;
        }
        
        private static string Example(string key, string val, bool expression)
        {
            
            string[] array= expression ? new[] {"%3C", "%3E", "%3C%2F", "%3E"} : new[] {"<", ">", "</", ">"};
            string result = $"{array[0]}{key}{array[1]}{val}{array[2]}{key}{array[3]}";
            return result;
        }

        public static string[] TextToLines(string json)
        {
            var lines = json.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
            return lines;
        }

        public static string LanguageFind(Method method, string lang)
        {
            string Language;
            switch (lang)
            {
                case "C# - HttpClient" :
                    Language = method.CsCode;
                    break;
                case "PHP – cURL" :
                    Language = method.Php1Code;
                    break;
                case "PHP – HTTP-Request2" :
                    Language = method.Php2Code;
                    break;
                case "Python – Requests" :
                    Language = method.PythonCode;
                    break;
                case "NodeJS – Native" :
                    Language = method.NodeJsCode;
                    break;
                default:
                    Language = "";
                    break;
            }
            return Language;
        }
        
    }