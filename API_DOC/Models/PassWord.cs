using System.ComponentModel.DataAnnotations;

namespace API_DOC.Models ;

    public class PassWord
    {
        [StringLength(100)]
        public string Past { get; set; } = "";

        [StringLength(100)]
        public string Current { get; set; } = "";

        [StringLength(100)]
        public string Confirm { get; set; } = "";
    }