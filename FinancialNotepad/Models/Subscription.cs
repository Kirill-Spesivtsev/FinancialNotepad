using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialNotepad.Models
{
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; } 
        [Required]
        public DateTime DateStart { get; set; }
        [Required]
        public DateTime DateEnd { get; set; }
        public int PayPrice { get; set; }
        public int CurrencyId { get; set; }

        public string UserId { get; set; } = "";

    }
}
