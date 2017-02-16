using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    /// <summary>
    /// CoursesContrloller represents resoursces belonging to courses
    /// </summary>

    [Route("api/courses")]
    public class CoursesController : Controller
    {
        private static List<Course> _courses;
        private static List<Student> _students;

        /// <summary>
        ///  This method creates new instances of students and courses
        /// </summary>
        public CoursesController()
        {
            if (_students == null)
            {
                _students = new List<Student>
                {
                    new Student
                    {
                        SSN = 1908892469,
                        Name = "Tanya Brá Brynjarsdóttir"
                    },
                    new Student
                    {
                        SSN = 0309942469,
                        Name = "Karen Björg Halldórsdóttir"
                    },
                    new Student
                    {
                        SSN = 1808892469,
                        Name = "Sigga Gunnarsdóttir"
                    },
                    new Student
                    {
                        SSN = 0509942469,
                        Name = "Jón Jónssson"
                    },
                    new Student
                    {
                        SSN = 1909892469,
                        Name = "Valli Valsson"
                    },
                    new Student
                    {
                        SSN = 0309902469,
                        Name = "Dóri Valsson"
                    }
                };
            }
            if (_courses == null)
            {
                _courses = new List<Course>
                {
                    new Course
                    {
                        ID         = 1,
                        Name       = "Web services",
                        TemplateID = "T-514-VEFT",
                        StartDate  = DateTime.Now,
                        EndDate    = DateTime.Now.AddMonths(3),
                        Students = new List<Student>()
                        {
                            _students[0],
                            _students[1]
                        }
                        
                    },
                    new Course
                    {
                        ID         = 2,
                        Name       = "Compilers",
                        TemplateID = "T-603-THYD",
                        StartDate  = DateTime.Now,
                        EndDate    = DateTime.Now.AddMonths(3),
                        Students = new List<Student>
                        {
                            _students[2],
                            _students[3]
                        }
                    },
                };
            }
        }
        
        /// <summary>
        /// This method returns all courses that are listed, if there are no courses listed
        /// an empty list is returned.
        /// </summary>
        // GET api/courses
        [HttpGet]
        [Route("")]
        public List<Course> Courses()
        {
            return _courses;
        }

        /// <summary>
        /// This method returns a course with the requested ID, if there is no course
        /// with the given ID then status code 404 is returned
        /// </summary>
        // GET api/courses/id
        [HttpGet]
        [Route("{id:int}", Name="GetCourse")]
        public IActionResult GetCourseByID(int id)
        {
            var course = _courses.FirstOrDefault((p) => p.ID == id);
            if(course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        /// <summary>
        /// This method creates a new course and returns status code 201, 
        /// if the new course does not contain an ID, name or
        /// the ID is already taken then status code 400 is returned. 
        /// </summary>
        // POST api/courses
        [HttpPost]
        public IActionResult AddCourse([FromBody] Course newCourse)
        {
            if (newCourse == null || newCourse.ID == null || newCourse.Name == null)
            {
                return BadRequest();
            }
            var course = _courses.FirstOrDefault((p) => p.ID == newCourse.ID);
            if(course != null)
            {
                return BadRequest();
            }
            _courses.Add(newCourse);
            var location = Url.Link("GetCourse", new { id = newCourse.ID });
            return Created(location, newCourse);
        }

        /// <summary>
        /// This method updates an existing course with the requested ID but only the fields that
        /// are not null and returns status code 200, if the course does not exist then
        /// status code 404 is returned.
        /// </summary>
        // PUT api/courses/id
        [HttpPut("{id:int}")]
        public IActionResult UpdateCorse(int id, [FromBody] Course updated)
        {
            var course = _courses.FirstOrDefault((p) => p.ID == id);
            if (course == null)
            {
                return NotFound();
            }
            if (updated == null)
            {
                return BadRequest();
            }
            if (updated.Name != null)
            {
                course.Name = updated.Name;
            }
            if (updated.TemplateID != null)
            {
                course.TemplateID = updated.TemplateID;
            }
            if (updated.StartDate != DateTime.MinValue)
            {
                course.StartDate = updated.StartDate;
            }
            if (updated.EndDate != DateTime.MinValue)
            {
                course.EndDate = updated.EndDate;
            }
            if (updated.Students != null)
            {
                course.Students = updated.Students;
            }
            return Ok(course);
        }

        /// <summary>
        /// This method deletes a course with the requested ID. If the course is not listed
        /// then status code 404 is returned.
        /// </summary>
        // DELETE api/courses/id
        [HttpDelete("{id:int}")]
        public IActionResult DeleteCourse(int id)
        {
            var courseToRemove = _courses.FirstOrDefault((p) => p.ID == id);
            if (courseToRemove == null)
            {
                return NotFound();
            } 
            _courses.Remove(courseToRemove);
            return NoContent();
        }

        /// <summary>
        /// This method returns a list of students in a course with reqested ID.
        /// </summary
        // GET api/courses/id/students>
        [HttpGet]
        [Route("{id}/students", Name = "GetStudentsOfCourse")]
        public IActionResult GeStudentsByID(int id)
        {
            var course = _courses.FirstOrDefault((p) => p.ID == id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course.Students);
        }

        /// <summary>
        /// This method adds a new students to course. If the student is not listed in the school
        /// (students list), the student is already in the course or required properties
        /// are missing then 400 is returned.
        /// </summary>
        // POST api/courses/id/students
        [HttpPost]
        [Route("{id}/students")]
        public IActionResult AddStudentToCourse(int id, [FromBody] Student newStudent)
         {
            var course = _courses.FirstOrDefault((p) => p.ID == id);
            if (newStudent == null || newStudent.SSN == null)
            {
                return BadRequest();
            }

            var inList = course.Students.FirstOrDefault((p) => p.SSN == newStudent.SSN);
            var student = _students.FirstOrDefault((p) => p.SSN == newStudent.SSN);

            if (inList != null || student == null)
            {
                return BadRequest();
            }

            course.Students.Add(newStudent);
            var location = Url.Link("GetStudentsOfCourse", id);
            return Created(location, course.Students);
         }

    }
}
