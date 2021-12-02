﻿using Microsoft.AspNetCore.Mvc;
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
            //ViewData["Student"] = dbStudent;
            return View(dbStudent);
        }

        [HttpPost]
        public IActionResult EditStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                Student dbStudent = GetStudent(student.StudentId);
                if (dbStudent != null)
                {
                    using (var transaction = studentsDb.Database.BeginTransaction())
                    {
                        try
                        {
                            studentsDb.Entry(dbStudent).CurrentValues.SetValues(student);
                            dbStudent.AcademicPerfomance.AcademicRecord = student.AcademicPerfomance.AcademicRecord; //Пока так, временный костыль.
                                                                                                                     //studentsDb.Entry(dbStudent.AcademicPerfomance).CurrentValues.SetValues(student.AcademicPerfomance); //error из-за попытки изменить primary key
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
            return View(student);
        }
        
        [HttpGet]
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
                        throw;
                    }
                    return Redirect("/");
                }
            }

            return View(student);
            
        }

      
        [HttpGet]
        [HttpPost]
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
