using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialNotepad.Models
{
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; }
        public int PayPrice { get; set; }
        public int CurrencyId { get; set; }

        public string SubId { get; set; }
        public string? CusId { get; set; }
        public string? ProdId { get; set; }
        public string? PriceId { get; set; }

        public DateTime DateStart { get; set; } = DateTime.Now;

        public string UserId { get; set; } = "";

    }
}
