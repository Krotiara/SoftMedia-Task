using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SoftMedia_Task.Controllers;
using SoftMedia_Task.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                List<Student> items = controller.GetStudentsList().Result;
                Assert.Equal(3, items.Count);
                Assert.Equal("First student", items[0].FullName);
                Assert.Equal("Second student", items[1].FullName);
                Assert.Equal("Third student", items[2].FullName);

            }
        }

        [Fact]
        public void AddTest()
        {
            using (var context = new StudentContext(dbContextOptions))
            {
                Student studentToAdd = new Student("Add test",
                    new DateTime(1905,1,1),
                    new AcademicPerfomance()
                    {
                        AcademicRecord = AcademicRecords.Excellent,
                    });

                HomeController controller = new HomeController(context);
                controller.AddStudent(studentToAdd);

                Student student = context.Students.Include(x => x.AcademicPerfomance).Last();
                Assert.Equal("Add test", student.FullName);
                Assert.Equal(AcademicRecords.Excellent, student.AcademicPerfomance.AcademicRecord);

            }
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
        public void DeleteTest()
        {
            using (var context = new StudentContext(dbContextOptions))
            {
                HomeController controller = new HomeController(context);
                Student student = context.Students.Include(x => x.AcademicPerfomance).Last();
                int idToDelete = student.StudentId;
                controller.DeleteStudent(idToDelete);
                Assert.Null(controller.GetStudent(idToDelete));
            }
        }

        [Fact]
        public void GetStudentTest()
        {
            using (var context = new StudentContext(dbContextOptions))
            {
                HomeController controller = new HomeController(context);
                Student existingStudent = controller.GetStudent(1);
                Student notExistingStudent = controller.GetStudent(100);
                Assert.NotNull(existingStudent);
                Assert.Null(notExistingStudent);
            }
        }

        [Fact]
        public void ViewsResultNotNullTest()
        {
            using (var context = new StudentContext(dbContextOptions))
            {
                HomeController controller = new HomeController(context);
                ViewResult indexResult = controller.Index() as ViewResult;
                ViewResult editResult = controller.EditStudent(1) as ViewResult;
                Assert.NotNull(indexResult);
                Assert.NotNull(editResult);


            }
        }

        [Fact]
        public void EditNotExistingStudentRedirectToIndexViewTest()
        {
            using (var context = new StudentContext(dbContextOptions))
            {
                HomeController controller = new HomeController(context);
                RedirectToActionResult editResult = controller.EditStudent(100) as RedirectToActionResult;    
                Assert.Equal("Index", editResult.ActionName);
            }
        }

        [Fact]
        public void IndexReturnsViewResultWithStudentsListTest()
        {
            using (var context = new StudentContext(dbContextOptions))
            {
                HomeController controller = new HomeController(context);
                ViewResult indexResult = controller.Index() as ViewResult;
                List<Student> students = indexResult.ViewData["Students"] as List<Student>;          
                Assert.Equal(controller.GetStudentsList().Result.Count, students.Count());

            }
        }

        [Fact]
        public void RedirectToIndexAfterDeleteTest()
        {
            using (var context = new StudentContext(dbContextOptions))
            {
                HomeController controller = new HomeController(context);
                Student student = context.Students.Include(x => x.AcademicPerfomance).Last();
                RedirectToActionResult editResult = controller.DeleteStudent(student.StudentId) as RedirectToActionResult;
                Assert.Equal("Index", editResult.ActionName);
            }
        }
    }
}
