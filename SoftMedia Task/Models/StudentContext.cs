using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftMedia_Task.Models
{
    public class StudentContext: DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<AcademicPerfomance> AcademicPerfomances { get; set; }

        public StudentContext(DbContextOptions<StudentContext> options): base(options)
        {
            Database.EnsureCreated();
        }
    }
}
