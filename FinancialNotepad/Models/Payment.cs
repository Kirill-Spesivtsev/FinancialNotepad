using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialNotepad.Models;

public class Payment
{
    [Key]
    public int TransactionId { get; set; }

    [Range(1,int.MaxValue,ErrorMessage ="Please select a category")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0")]
    public int Amount { get; set; }

    public int CurrencyId { get; set; }

    public int TaxId { get; set; }

    [Column(TypeName = "nvarchar(75)")]
    public string? Note { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;
}