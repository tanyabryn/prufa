
using System;
using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models
{
    public class Student
    {
        [Required]
        public int? SSN { get; set; }
        public string Name { get; set; }
    }
}