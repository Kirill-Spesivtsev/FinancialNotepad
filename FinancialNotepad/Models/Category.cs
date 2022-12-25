using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FinancialNotepad.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Column(TypeName = "nvarchar(30)")]
    [Required(ErrorMessage = "Title is necessary")]
    public string Title { get; set; }

    [Column(TypeName = "nvarchar(20)")]
    public string? Icon { get; set; } = "";

    public string UserId { get; set; } = "";

    [JsonIgnore]
    public List<Transaction> Transactions { get; set; }

    [NotMapped]
    public string? TitleAndIcon
    {
        get
        {
            if (Icon == null || Icon == "")
            {
                return Title;
            }
            else
            {
                return Icon + " " + Title;
            }
        }
    }
}