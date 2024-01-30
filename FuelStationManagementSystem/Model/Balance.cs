using FuelStationManagementSystem.Helpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelStationManagementSystem.Models
{
    [Table("Balance")]
    public class Balance
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public double Amount { get; set; }

        public BalanceType Type { get; set; }

        public string? CustomerTCKN { get; set; }

        public Customer Customer { get; set; }

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
