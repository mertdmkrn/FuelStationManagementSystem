using FuelStationManagementSystem.Helpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelStationManagementSystem.Models
{
    [Table("Vehicle")]
    public class Vehicle
    {
        [Key]
        [MinLength(5)]
        [MaxLength(20)]
        public string Plate { get; set; }

        public VehicleType VehicleType { get; set; }
        public FuelType FuelType { get; set; }

        [Required]
        public string? CustomerTCKN { get; set; }

        public Customer Customer { get; set; }
    }
}
