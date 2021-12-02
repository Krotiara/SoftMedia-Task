using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftMedia_Task.Models
{
    [Table("Students")]
    public class Student
    {
        public Student()
        {
            //suitable constructor for entity type
        }

        public Student(string name, DateTime birthdate, AcademicPerfomance academicPerfomance)
        {
            FullName = name;
            Birthdate = birthdate;
            AcademicPerfomance = academicPerfomance;
            AcademicPerfomance.Student = this;
        }

        [Key]
        [Column("Id")]
        public int StudentId { get; set; }

        [Column("Name")]
        [Display(Name = "Name")]
        [Required]
        public string FullName { get; set; }

        [Column("Birthdate", TypeName = "Date")]
        public DateTime Birthdate { get; set; }

        public AcademicPerfomance AcademicPerfomance { get; set; }


      
    }
}
