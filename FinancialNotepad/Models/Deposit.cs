using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialNotepad.Models;

public class Deposit
{
    [Key]
    public int DepositId { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string Description { get; set; }

    [Column(TypeName = "nvarchar(20)")]
    public string Status { get; set; } = "Active";

    [Required]
    [Column(TypeName = "nvarchar(10)")]
    public double TotalSum { get; set; }

    [Required]
    public double AnnualPercent { get; set; }

    [Required]
    public int Period { get; set; }

    public double LeftToPay { get; set; }
}