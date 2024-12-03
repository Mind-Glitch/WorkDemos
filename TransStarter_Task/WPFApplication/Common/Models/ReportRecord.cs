namespace TransStarter_Task.WPFApplication.Common.Models;
public class ReportRecord
{
    public int MonthIndex { get; init; }
    public int YearIndex { get; init; }
    public string ItemName { get; init; } = string.Empty;
    public double Income { get; set; }

    public string DisplayData { get; set; } = string.Empty;

    public string IncomeString => Income.ToString("N0");
}