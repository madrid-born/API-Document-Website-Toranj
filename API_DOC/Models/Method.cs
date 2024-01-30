using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_DOC.Models;

public class Method
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [StringLength(100)]
    public string Name { get; set; } = null!;
    [StringLength(200)]
    public string Details { get; set; } = null!;
    [StringLength(500)]
    public string Key { get; set; } = null!;
    [StringLength(1500)]
    public string CsCode { get; set; } = "";
    [StringLength(1500)]
    public string Php1Code { get; set; } = "";
    [StringLength(1500)]
    public string Php2Code { get; set; } = "";
    [StringLength(1500)]
    public string PythonCode { get; set; } = "";
    [StringLength(1500)]
    public string NodeJsCode { get; set; } = "";
    [StringLength(1500)]
    public string PersianName { get; set; } = "";
    [StringLength(1500)]
    public string OutPut { get; set; } = "";
}