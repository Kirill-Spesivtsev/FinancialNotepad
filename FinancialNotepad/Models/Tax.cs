using System.ComponentModel.DataAnnotations;

namespace FinancialNotepad.Models;

public class Tax
{
    [Key]
    public int TaxId { get; set; }

    public int Title { get; set; }

    public int Description { get; set; }

}