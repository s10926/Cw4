using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private const String ConString = "Data Source=db-mssql;Initial Catalog=s10926;Integrated Security=True";
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
        public IActionResult getStudents()
        {
            var list = new List<Student>();

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT * FROM students";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    list.Add(st);
                }
            }

            return Ok(list);
        }

        [HttpGet]
        public IActionResult getStudents(String orderBy)
        {
            //return $"Kowalski, Malewski, Andrzejewski sortowanie={orderBy}";
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("{indexNumber}")]
        public IActionResult getStudentGrades(String indexNumber)
        {
            var list = new List<int>();

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT g.grade" +
                                  "FROM   grades g" +
                                  "JOIN   students s" +
                                  "ON     g.IndexNumber = s.IndexNumber" +
                                  "WHERE  g.IndexNumber = @index" +
                                  "AND    g.Date BETWEEN \'2020-02-29\' AND \'2020-09-25\'";

                SqlParameter par = new SqlParameter();
                par.Value = indexNumber;
                par.ParameterName = "index";
                com.Parameters.Add(par);

                //com.Parameters.AddWithValue("index", indexNumber);

                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var grade = dr.GetInt32(0);
                    list.Add(grade);
                }
            }

            return Ok(list);
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
