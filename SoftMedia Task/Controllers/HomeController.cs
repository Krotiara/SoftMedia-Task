using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SoftMedia_Task.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SoftMedia_Task.Controllers
{
    //[Route("[controller]")]
    public class HomeController : Controller
    {
        StudentContext studentsDb;
        public HomeController(StudentContext context)
        {
            studentsDb = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // IEnumerable<StudentDto> items = GetStudentsList().Result.Value;
            ViewData["Students"] = studentsDb.Students.Include(x => x.AcademicPerfomance).ToList(); //Async todo
            //return View("Index", GetStudentsList().Result.Value);
            return View("Index");
        }

        [HttpGet, Route("data")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsList()
        {
            //var items = await studentsDb.Students.Join(studentsDb.AcademicPerfomances, u => u.AcademicPerfomance.Id, c => c.Id,
            //    (u, c) => new StudentDto
            //    {
            //        Id = u.Id,
            //        FullName = u.FullName,
            //        Birthdate = u.Birthdate.Date,
            //        AcademicRecord = c.AcademicRecord,
            //    }).ToListAsync();
            List<Student> list = await studentsDb.Students.Include(x => x.AcademicPerfomance).ToListAsync();
            return list;
        }

        

        [HttpGet, Route("edit/{id}")]
        public IActionResult EditStudent(int id)
        {
            Student dbStudent = studentsDb.Students.Include(x => x.AcademicPerfomance).SingleOrDefault(s => s.StudentId == id);
            if (dbStudent == null)
                return RedirectToAction("Index");
            ViewData["Student"] = dbStudent;
            ViewData["Action mode"] = "edit";
            return View("EditStudent");
        }



        [HttpPost]
        public IActionResult EditStudent(Student student, AcademicPerfomance academicPerfomance)
        {
            Student dbStudent = studentsDb.Students.Include(x => x.AcademicPerfomance).SingleOrDefault(s => s.StudentId == student.StudentId);
            if (dbStudent != null)
            {
                using (var transaction = studentsDb.Database.BeginTransaction())
                {
                    try
                    {   
                        studentsDb.Entry(dbStudent).CurrentValues.SetValues(student);
                        dbStudent.AcademicPerfomance.AcademicRecord = academicPerfomance.AcademicRecord; //Пока так, разобраться с прокидыванием ссылок в параметры
                        studentsDb.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return Redirect("/");
        }

        //[HttpPost]
        //public async IActionResult AddStudent(Student student, AcademicPerfomance studentPerfomance)
        //{
        //    using(var transaction = studentsDb.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            studentsDb.Add(student);
        //            studentsDb.Add(studentPerfomance);
        //            await studentsDb.SaveChangesAsync(); //После этого происходит автоинкремент
        //            student
        //        }
        //        catch(Exception e)
        //        {
        //            transaction.Rollback();
        //        }
        //    }
        //}


    }
}
