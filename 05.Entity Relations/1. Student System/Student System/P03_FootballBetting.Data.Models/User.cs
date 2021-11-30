using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
   public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(60)]     
        public string Username { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }
        [Required]
        [MaxLength(75)]
        public string Email { get; set; }
        [Required]
        [MaxLength(75)]
        public string Name { get; set; }

        public decimal Balance { get; set; }
    }
}
