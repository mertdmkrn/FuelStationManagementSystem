using FuelStationManagementSystem.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelStationManagementSystem.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        [MaxLength(11)]
        public string TCKN { get; set; }

        [MaxLength(100)]
        public string? NameSurname { get; set; }
        
        [MaxLength(200)]
        public string? Address { get; set; }

        public CustomerStatus Status { get; set; }
    }
}
