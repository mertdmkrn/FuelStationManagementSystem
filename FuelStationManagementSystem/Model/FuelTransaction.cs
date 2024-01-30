using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FuelStationManagementSystem.Models
{
    [Table("FuelTransaction")]
    public class FuelTransaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public double Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        [MaxLength(20)]
        public string? VehiclePlate { get; set; }

        [JsonIgnore]
        public Vehicle? Vehicle { get; set; }
    }
}
