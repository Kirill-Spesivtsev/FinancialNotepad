using System.ComponentModel.DataAnnotations;

namespace FinancialNotepad.Models;

public class Currency
{
    [Key]
    public int CurrencyId { get; set; }

    [Required]
    [StringLength(40)]
    public string Name { get; set; }

    public char Symbol { get; set; }
}