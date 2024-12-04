using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransStarter_Task.WPFApplication.Common.Entity
{
    public class VehicleInfo
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Марка")]
        [Column("brand_name", TypeName = "NVARCHAR(128)")]
        public string BrandName { get; set; } = string.Empty;

        [DisplayName("Модель")]
        [Column("model_name", TypeName = "NVARCHAR(128)")]
        public string ModelName { get; set; } = string.Empty;

        [DisplayName("Цвет")]
        [Column("color", TypeName = "NVARCHAR(64)")]
        public string Color { get; set; } = string.Empty;

        [DisplayName("Комплектация")]
        [Column("equipment", TypeName = "NVARCHAR(1024)")]
        public string Equipment { get; set; } = string.Empty;

        public virtual List<Car>? Cars { get; set; } = [];

        public string GetVehicleFullName() => $"{BrandName} {ModelName} {Color} {Equipment}";
    }
}
