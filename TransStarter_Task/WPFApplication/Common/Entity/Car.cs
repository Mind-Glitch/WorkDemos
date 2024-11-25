using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TransStarter_Task.WPFApplication.Common.Entity;
public class Car
{
    [Key]
    public Guid Id { get; set; }

    [DisplayName("Цена")]
    public double Price { get; set; }

    /// <summary>
    /// Добавил просто так
    /// </summary>
    [DisplayName("S/N")]
    public string SN { get; set; } = string.Empty;

    public virtual VehicleInfo? VehicleInfo { get; set; }
}