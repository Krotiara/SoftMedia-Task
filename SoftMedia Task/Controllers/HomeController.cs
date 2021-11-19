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
          
            return View("Index", GetStudentsList().Result.Value);
        }

        [HttpGet, Route("data")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsList()
        {
            var items = await studentsDb.Students.Join(studentsDb.AcademicPerfomances, u => u.Id, c => c.StudentId,
                (u, c) => new StudentDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Birthdate = u.Birthdate.Date,
                    AcademicRecord = c.AcademicRecord
                }).ToListAsync();
            return items;
        }

        

        [HttpGet, Route("edit")]
        public IActionResult EditStudent(int id)
        {
            Student dbStudent = studentsDb.Students.SingleOrDefault(s => s.Id == id);
            AcademicPerfomance dbAperf = studentsDb.AcademicPerfomances.SingleOrDefault(s => s.StudentId == id);
            if (dbStudent == null || dbAperf == null)
                return RedirectToAction("Index");
            ViewData["Student"] = dbStudent;
            ViewData["Academic record"] = dbAperf;
            return View("EditStudent");
        }

        [HttpPost]
        public IActionResult EditStudent(int id, Student student, AcademicPerfomance studentPerfomance)
        {
            Student dbStudent = studentsDb.Students.SingleOrDefault(s => s.Id == id);
            AcademicPerfomance dbAperf = studentsDb.AcademicPerfomances.SingleOrDefault(s => s.StudentId == id);
            if (dbStudent != null && dbAperf != null)
            {
                using (var transaction = studentsDb.Database.BeginTransaction())
                {
                    try
                    {
                        
                        studentsDb.Entry(dbStudent).CurrentValues.SetValues(student);
                        studentsDb.Entry(dbAperf).CurrentValues.SetValues(studentPerfomance);
                        studentsDb.SaveChanges();
                        transaction.Commit();
                    }
                    catch(Exception e)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return Index(); //Сам Url не меняется TODO
        }


    }
}
