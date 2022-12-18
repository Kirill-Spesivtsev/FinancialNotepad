using System.ComponentModel.DataAnnotations;

namespace FinancialNotepad.Models;

public class Tax
{
    [Key]
    public int TaxId { get; set; }

    public string Title { get; set; }

    public double Percent { get; set; }

    public string Description { get; set; }

}