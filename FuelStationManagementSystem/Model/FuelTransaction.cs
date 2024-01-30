using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelStationManagementSystem.Models
{
    [Table("FuelTransaction")]
    public class FuelTransaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }

        public string? VehiclePlate { get; set; }

        public Vehicle Vehicle { get; set; }
    }
}
