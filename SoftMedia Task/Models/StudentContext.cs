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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Помните, что аннотации позволяют простым образом настраивать конфигурацию Entity Framework,
            //но при этом, с помощью аннотаций нельзя настроить некоторые сложные моменты, для которых используется Fluent API.
            modelBuilder.Entity<Student>()
                .HasOne<AcademicPerfomance>(s => s.AcademicPerfomance)
                .WithOne(a => a.Student)
                .HasForeignKey<AcademicPerfomance>(a => a.StudentId)
                .IsRequired();
           
           
            modelBuilder.Entity<Student>()
                .Property(s => s.StudentId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<AcademicPerfomance>()
               .Property(p => p.AcademicRecordID)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<AcademicPerfomance>()
                .Property(p => p.AcademicRecord)
                .HasConversion<string>(); //Пока так, если без descriptions.

            //https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key
            //https://metanit.com/sharp/entityframeworkcore/2.13.php




        }
    }
}
