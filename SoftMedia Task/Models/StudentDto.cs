using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftMedia_Task.Models
{
    /// <summary>
    /// Объект передачи данных всех информации о студенте
    /// </summary>
    public class StudentDto
    {
        public int Id { get; set; }
     
        public string FullName { get; set; }
     
        public DateTime Birthdate { get; set; }

        public string AcademicRecord { get; set; }
    }
}
