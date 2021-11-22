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
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)] //база данных сама генерирует значение
        [Column("Id")]
        public int StudentId { get; set; }
        [Column("Name")]
        [Display(Name = "Name")]
        public string FullName { get; set; }
        [Column("Birthdate", TypeName = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")] //not working
        public DateTime Birthdate { get; set; }


        //внещний ключ

        // Платформа Entity Framework не требует добавлять свойство внешнего ключа в модель данных при наличии свойства навигации для связанной сущности.
        //Свойство навигации: ICollection<T>: https://docs.microsoft.com/ru-ru/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-a-more-complex-data-model-for-an-asp-net-mvc-application
        //public int AcademicRecordID { get; set; }
        public AcademicPerfomance AcademicPerfomance { get; set; }
    }
}
