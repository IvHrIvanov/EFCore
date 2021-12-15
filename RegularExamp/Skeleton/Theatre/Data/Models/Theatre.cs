using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.Data.Models
{
    public class Theatre
    {
        public Theatre()
        {
            this.Tickets = new HashSet<Ticket>();
        }
        [Key]
        public int Id { get; set; }

        [Range(4,30)]
        [Required]
        public string Name { get; set; }
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [Range(4,30)]
        public string Director { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

    }
}
