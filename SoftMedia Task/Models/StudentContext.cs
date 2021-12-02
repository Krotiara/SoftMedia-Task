using Microsoft.EntityFrameworkCore;


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
            modelBuilder.Entity<Student>()
                .HasOne<AcademicPerfomance>(s => s.AcademicPerfomance)
                .WithOne()
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
                .HasConversion<string>();
   
        }
    }
}
