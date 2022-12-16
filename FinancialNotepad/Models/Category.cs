using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialNotepad.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    [Required(ErrorMessage = "Title is necessary")]
    public string Title { get; set; }

    [Column(TypeName = "nvarchar(20)")]
    public string? Icon { get; set; } = "";

    [Column(TypeName = "nvarchar(10)")]
    public string Type { get; set; } = "Expense";

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