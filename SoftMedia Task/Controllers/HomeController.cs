using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftMedia_Task.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftMedia_Task.Controllers
{
    public class HomeController : Controller
    {
        readonly StudentContext studentsDb;
        public HomeController(StudentContext context)
        {
            //Getting the dbcontext occurs through DI into the constructor.  
            studentsDb = context;
        }

        /// <summary>
        /// Show all students with their attributes.
        /// </summary>
        /// <returns>return View with student`s table.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Students"] = GetStudentsList().Result;
            return View("Index");
        }


        [HttpGet, Route("data")]
        public async Task<List<Student>> GetStudentsList()
        {
            List<Student> list = await studentsDb.Students.Include(x => x.AcademicPerfomance).ToListAsync();
            return list;
        }

        
        public Student GetStudent(int id)
        {
            return studentsDb.Students.Include(x => x.AcademicPerfomance).SingleOrDefault(s => s.StudentId == id);
        }


        [HttpGet, Route("edit/{id}")]
        public IActionResult EditStudent(int id)
        {
            Student dbStudent = GetStudent(id);
            if (dbStudent == null)
                return RedirectToAction("Index");
            return View(dbStudent);
        }

        [HttpPost]
        public IActionResult EditStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = studentsDb.Database.BeginTransaction()) 
                //После рефакторинга по идее транзакции уже не требуется, т.к. уже нет последовательности операций, а только Update.
                {
                    try
                    {
                        studentsDb.Update(student);
                        studentsDb.SaveChanges();
                        transaction.Commit();

                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }

                }
                return Redirect("/");
            }
            return View(student);
        }
        
        [HttpGet, Route("add")]
        public IActionResult AddStudent()
        {
            return View("AddStudent");
        }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = studentsDb.Database.BeginTransaction())
                {
                    try
                    {
                        studentsDb.Students.Add(student);
                        studentsDb.AcademicPerfomances.Add(student.AcademicPerfomance);
                        studentsDb.SaveChanges();
                        transaction.Commit();

                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                    return Redirect("/");
                }
            }

            return View(student);
            
        }

      
        [HttpGet] //Появилась из-за @Html.ActionLink (в Index View), который создает только Get запрос.
        [HttpPost]
        [Route("delete/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            using (var transaction = studentsDb.Database.BeginTransaction())
            {
                try
                {
                    Student student = GetStudent(id);
                    studentsDb.AcademicPerfomances.Remove(student.AcademicPerfomance);
                    studentsDb.Students.Remove(student);
                    studentsDb.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GoToSwaggerUI()
        {
            return Redirect("/swagger");
        }
    }
}
