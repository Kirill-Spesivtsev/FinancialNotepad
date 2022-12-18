using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialNotepad.Models;

public class Transaction
{
    [Key]
    public int TransactionId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0")]
    public double Amount { get; set; }

    [Column(TypeName = "nvarchar(60)")]
    public string? Note { get; set; }

    [Column(TypeName = "nvarchar(10)")] 
    public string Type { get; set; } = "Expense";

    public DateTime Date { get; set; } = DateTime.Now;


    [Range(1,int.MaxValue,ErrorMessage ="Please select a category")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public int TaxId { get; set; } = 0;
    public Tax? Tax { get; set; }

    [Range(1,int.MaxValue,ErrorMessage ="Please select currency")]
    public int CurrencyId { get; set; }
    public Currency Currency { get; set; }

    [NotMapped]
    public string? CategoryTitleAndIcon
    {
        get
        {
            if (string.IsNullOrEmpty(Category?.Icon))
            {
                return Category?.Title;
            }
            else
            {
                return Category?.Icon + " " + Category?.Title;
            }
        }
    }

    [NotMapped]
    public string? FormattedAmount
    {
        get
        {
            return (Type == "Expense" ? "- " : "+ ") + Amount.ToString("F") + " " + Currency?.Symbol;
        }
    }

}