using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.DataProcessor.ImportDto
{
    public class TheatreInputDto
    {
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Name { get; set; }
        [Range(1, 10)]
        [Required]
        public sbyte NumberOfHalls { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Director { get; set; }

        public TicketsInputModel[] Tickets { get; set; }
    }
    public class TicketsInputModel
    {
        [Range(1.00, 100.00)]
        public decimal Price { get; set; }
        [Range(1, 10)]
        [Required]
        public sbyte RowNumber { get; set; }
       
    }
}

