using FuelStationManagementSystem.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FuelStationManagementSystem.Models
{
    [Table("Vehicle")]
    public class Vehicle
    {
        [Key]
        [MaxLength(20)]
        public string Plate { get; set; }

        public VehicleType VehicleType { get; set; }
        public FuelType FuelType { get; set; }

        [MaxLength(11)]
        public string? CustomerTCKN { get; set; }

        [JsonIgnore]
        public Customer? Customer { get; set; }
    }
}
