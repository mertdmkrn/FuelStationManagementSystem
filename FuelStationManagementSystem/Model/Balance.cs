using FuelStationManagementSystem.Helpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FuelStationManagementSystem.Models
{
    [Table("Balance")]
    public class Balance
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public double Amount { get; set; }

        public BalanceType Type { get; set; }

        [MaxLength(11)]
        public string? CustomerTCKN { get; set; }

        [JsonIgnore]
        public Customer? Customer { get; set; }

        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public Guid Id { get; set; }

        //[Required]
        //public double Amount { get; set; }

        //public BalanceType Type { get; set; }

        //public Guid? CustomerId { get; set; }

        //public Customer Customer { get; set; }

        //[NotMapped]
        //public string? CustomerTCKN { get; set; }
    }
}
