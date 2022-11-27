using System.ComponentModel.DataAnnotations;

namespace FinancialNotepad.Models;

public class Currency
{
    [Key]
    public int CurrencyId { get; set; }

    public int Name { get; set; }
}