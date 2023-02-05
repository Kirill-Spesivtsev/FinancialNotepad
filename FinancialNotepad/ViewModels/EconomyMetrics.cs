using System.ComponentModel.DataAnnotations;

namespace FinancialNotepad.ViewModels;

public class EconomyMetrics
{
    [Required]
    public double Income { get; set; }

    [Required]
    public double Expense { get; set; }

    [Required]
    public double VariableCosts { get; set; }
    
    [Required]
    [Range(1, 100)]
    public int CommonTaxPercent { get; set; }

    [Required]
    [Range(1, 10000)]
    public int UsefulPeriod { get; set; }

    [Required]
    public int AmSumCosts { get; set; }

    public double Profit { get; set; }
    public double MrjIncome { get; set; }
    public double AmDed { get; set; }
    public double PureProfit { get; set; }

    public bool Filled { get; set; } = false;
}