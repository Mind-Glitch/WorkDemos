namespace TransStarter_Task.WPFApplication.Common.Models;
internal class ReportRecord
{
    public int MounthIndex { get; init; }
    public int YearIndex { get; init; }
    public string ItemName { get; init; } = string.Empty;
    public double Income { get; set; }
}