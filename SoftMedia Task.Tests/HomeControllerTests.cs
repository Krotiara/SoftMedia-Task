using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SoftMedia_Task.Controllers;
using SoftMedia_Task.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SoftMedia_Task.Tests
{
    public class HomeControllerTests
    {

        public readonly DbContextOptions<StudentContext> dbContextOptions;

        public HomeControllerTests()
        {
            dbContextOptions = new DbContextOptionsBuilder<StudentContext>()
                .UseInMemoryDatabase("StudentsDB")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)) //Transactions are not supported by the in-memory store.
                .Options;

            ResetTestDB();
        }

        private void ResetTestDB()
        {
            using(var context = new StudentContext(dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                AcademicPerfomance firstAP = new AcademicPerfomance() { AcademicRecord = AcademicRecords.Good };
                Student first = new Student("First student", new DateTime(1997, 12, 30), firstAP);

                AcademicPerfomance secondAP = new AcademicPerfomance() { AcademicRecord = AcademicRecords.Excellent};
                Student second = new Student("Second student", new DateTime(2000, 5, 10), secondAP);

                AcademicPerfomance thirdAP = new AcademicPerfomance() { AcademicRecord = AcademicRecords.Satisfactorily };
                Student third = new Student("Third student", new DateTime(2000, 5, 10), thirdAP);

                context.Students.AddRange(first, second, third);
                context.AcademicPerfomances.AddRange(firstAP, secondAP, thirdAP);
                context.SaveChanges();
            }
        }

        [Fact]
        public void GetStudentsListTest()
        {
            using (var context = new StudentContext(dbContextOptions))
            {
                HomeController controller = new HomeController(context);
                List<Student> items = controller.GetStudentsList().Result.Value.ToList();
                Assert.Equal(3, items.Count());
                Assert.Equal("First student", items[0].FullName);
                Assert.Equal("Second student", items[1].FullName);
                Assert.Equal("Third student", items[2].FullName);

            }
        }

        [Fact]
        public void AddTest()
        {
            Assert.True(false); //TODO
        }

        [Fact]
        public void EditTest()
        {
            using (var context = new StudentContext(dbContextOptions))
            {
                Student student = context.Students.Include(x => x.AcademicPerfomance).First();

                Student studentToEdit = new Student("Edit test",
                    student.Birthdate,
                    new AcademicPerfomance()
                    {
                        AcademicRecord = AcademicRecords.Bad,
                        AcademicRecordID = student.AcademicPerfomance.AcademicRecordID,
                        StudentId = student.StudentId,
                    });
                studentToEdit.StudentId = student.StudentId;
                
               
                HomeController controller = new HomeController(context);
                controller.EditStudent(studentToEdit);

                Student editedStudent = context.Students.Include(x => x.AcademicPerfomance).First();
                Assert.Equal("Edit test", editedStudent.FullName);
                Assert.Equal(AcademicRecords.Bad, editedStudent.AcademicPerfomance.AcademicRecord);
            }
        }

        [Fact]
        public void DeleteTest() //TODO
        {
            Assert.True(false);
        }

    }
}
