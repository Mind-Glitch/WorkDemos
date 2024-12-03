using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransStarter_Task.WPFApplication.Common.Entity;
public class Order
{
    [Key]
    public Guid Id { get; set; }
    
    [Column("customer")]
    public Customer? Customer { get; set; }

    [Column("car")]
    public List<Car> Cars { get; set; } = new();

    [Column("is_completed")]
    public bool Completed { get; set; }

    /// <summary>
    /// Проще загрузить только TotalPrice, чем подгружать Car и в рантайме считать
    ///</summary>
    [Column("total_price")]
    public double TotalPrice { get; set; }

    [Column("order_date")]
    public DateTime OrderDate { get; set; }
}