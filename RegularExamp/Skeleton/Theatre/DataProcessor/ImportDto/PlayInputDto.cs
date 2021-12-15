using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class PlayInputDto
    {
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        [XmlElement("Title")]
        public string Title { get; set; }
        [XmlElement("Duration")]
        public string Duration { get; set; }
        [Range(0.00, 10.00)]
        [XmlElement("Rating")]
        public float Rating { get; set; }
        [EnumDataType(typeof(Genre))]
        [Required]
        [XmlElement("Genre")]
        public string Genre { get; set; }
        [Required]
        [MaxLength(700)]
        [XmlElement("Description")]
        public string Description { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        [XmlElement("Screenwriter")]
        public string Screenwriter { get; set; }
    }
}
