using System.ComponentModel.DataAnnotations;

namespace TransStarter_Task.WPFApplication.Common.Entity;
public class Customer
{
    [Key]
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public List<Order> Orders { get; set; } = [];

}