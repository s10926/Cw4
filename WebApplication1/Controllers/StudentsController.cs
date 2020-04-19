using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw3.DAL;
using Cw3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }
        /*
        [HttpGet]
        public String getStudent()
        {
            return "Kowalski, Malewski, Andrzejewski";
        }
        */

        [HttpGet("{id}")]
        public IActionResult getStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Kowalski");
            } else if (id == 2)
            {
                return Ok("Malewski");
            }

            return NotFound("Nie znaleziono studenta.");
        }

        [HttpGet]
        public IActionResult getStudents(String orderBy)
        {
            //return $"Kowalski, Malewski, Andrzejewski sortowanie={orderBy}";
            return Ok(_dbService.GetStudents());
        }

        [HttpPost]
        public IActionResult createStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpDelete("{id}")]
        public IActionResult removeStudent(int id)
        {
            return Ok($"Student {id} został usunięty.");
        }

        [HttpPut("{id}")]
        public IActionResult updateStudent(int id)
        {
            return Ok($"Student {id} został zaktualizowany.");
        }
    }
}
