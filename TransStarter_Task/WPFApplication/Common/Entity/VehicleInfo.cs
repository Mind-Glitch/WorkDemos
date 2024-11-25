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
        public string BrandName { get; set; } = string.Empty;

        [DisplayName("Модель")]
        public string ModelName { get; set; } = string.Empty;

        public virtual List<Car>? Cars { get; set; }

        public string GetVehicleFullName() => $"{BrandName} {ModelName}";
    }
}
