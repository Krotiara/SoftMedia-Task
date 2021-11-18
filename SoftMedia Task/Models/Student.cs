using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftMedia_Task.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Column("Name")]
        public string FullName { get; set; }
        [Column("Birthdate", TypeName = "Date")]
        public DateTime Birthdate { get; set; }
    }
}
