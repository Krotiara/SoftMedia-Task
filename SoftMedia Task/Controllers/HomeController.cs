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
    [Route("[controller]")]
    public class HomeController : Controller
    {
        StudentContext studentsDb;
        public HomeController(StudentContext context)
        {
            studentsDb = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsList()
        {
            var items = await studentsDb.Students.Join(studentsDb.AcademicPerfomances, u => u.Id, c => c.StudentId,
                (u, c) => new StudentDto
                {
                    FullName = u.FullName,
                    Birthdate = u.Birthdate.Date,
                    AcademicRecord = c.AcademicRecord
                }).ToListAsync();
            return items;
        }


    }
}
