using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftMedia_Task.Models
{
    [Table("Students academic perfomances")]
    public class AcademicPerfomance
    {

        [Key]
        [Column("Id")]
        public int AcademicRecordID { get; set; }

        [Display(Name = "Academic perfomance")]
        [Column("Academic perfomance")]
        public AcademicRecords AcademicRecord { get; set; }

        //Внешний ключ
        [Column("StudentId")]
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }

    public enum AcademicRecords
    {
        Bad,
        Satisfactorily,
        Good,
        Excellent
    }
}
