using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftMedia_Task.Models
{
    [Table("Students academic perfomances")]
    public class AcademicPerfomance
    {
        [Key]
        [Column("Student id")]
        public int StudentId { get; set; }

        [Column("Academic perfomance")]
        public string AcademicRecord { get; set; }
    }
}
