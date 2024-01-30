using System.ComponentModel.DataAnnotations;

namespace API_DOC.Models ;

    public class ErrorMessage
    {
        [StringLength(2500)]
        public string Msg { get; set; } = "";

    }