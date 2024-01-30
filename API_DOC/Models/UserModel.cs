using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API_DOC.Models ;

    public class UserModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Username { get; set; } = "";

        [StringLength(100)]
        public string Password { get; set; } = "";

        [StringLength(100)]
        public string Email { get; set; } = "Dadehfa@info.com";

        [StringLength(100)]
        public string BrandName { get; set; } = "DadehFa";

        [StringLength(100)]
        public string Phone { get; set; } = "09123456789";

        [StringLength(1500)]
        public string Token { get; set; } = "";

        public bool Acceptance { get; set; } = false;
        
    }
