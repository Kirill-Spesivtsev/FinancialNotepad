using System.ComponentModel.DataAnnotations;

namespace FinancialNotepad.Models;

public class CheckingAccount
{
    [Key]
    public int CheckingAccountId { get; set; }

    [Required]
    public string BankAccount { get; set; }

    public string OwnerName { get; set; }

    public string BankName { get; set; } = "";
}