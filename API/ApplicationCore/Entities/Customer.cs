﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Entities
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? CCCD { get; set; }
        public string? Address { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public bool? FullInfo { get; set; }
        public bool? IsDeleted { get; set; }

        public string? RefreshToken { get; set; } 
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}