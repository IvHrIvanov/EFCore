using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
    public class ImportProjcetTaskDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        public string TaskOpenDate { get; set; }

        [XmlElement("DueDate")]
        public string TaskDueDate { get; set; }

        [XmlElement("ExecutionType")]
        [Range(0,3)]
        public int ExecutionType { get; set; }

        [XmlElement("LabelType")]
        [Range(0,4)]
        public int LabelType { get; set; }

    }
}
