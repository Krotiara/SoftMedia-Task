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
        [Column("Id")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)] //база данных сама генерирует значение
        public int AcademicRecordID { get; set; }

        [Display(Name = "Academic perfomance")]
        [Column("Academic perfomance")]
        public string AcademicRecord { get; set; }

        //Внешний ключ
        [Column("StudentId")]
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
