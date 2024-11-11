using Microsoft.AspNetCore.Mvc;
using StudentAPIBusinessLayer.Entities;
using StudentAPIBusinessLayer.Repositories;

namespace StudentApi.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly StudentRepository _studentRepository;

        public StudentAPIController(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
            return Ok(await _studentRepository.GetAllStudents());
        }


        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Student>>> GetPassedStudents()
        {
            return Ok(await _studentRepository.GetPassedStudents());
        }

        [HttpGet("Avg", Name = "AvgGradeRoute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<float>> GetAvgGrade()
        {
            if (await _studentRepository.GetStudentsCount() == 0)
            {
                return NotFound("No student found.");
            }
            return Ok(await _studentRepository.GetAverageGrade());
        }

        [HttpPost("TotalStudents", Name = "TotalStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetNumberOfTotalStudents()
        {
            int count= await _studentRepository.GetStudentsCount();
            if ( count == 0)
            {
                return NotFound("No student found.");
            }
            return Ok(count);
        }

        [HttpPost("RedirectToAvgGrade")]
        public IActionResult RedirectToAvgGrade()
        {
            return RedirectToRoute("AvgGradeRoute");
        }


        [HttpGet("{id}", Name = "StudenyByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID: {id}");
            }
            var student = await _studentRepository.GetStudentByID(id);
            if (student == null)
            {
                return NotFound("Student was not found");
            }
            return Ok(student);
        }

        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Student>> AddStudent(Student newStudent)
        {
            if (newStudent == null || string.IsNullOrEmpty(newStudent.Name)
                || newStudent.Age < 0 || newStudent.Grade < 0 || newStudent.Grade > 100)
            {
                return BadRequest("Invalid student data.");
            }

            newStudent=await _studentRepository.AddStudent(newStudent);
            return CreatedAtRoute("StudenyByID", new { id = newStudent.Id }, newStudent);
        }


        [HttpDelete("{id}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID: {id}");
            }
            var student = await _studentRepository.GetStudentByID(id);
            if (student == null)
            {
                return NotFound($"Student with Id {id} was not found");
            }
            await _studentRepository.DeleteStudent(id);
            return Ok($"Student with {id} has been removed");
        }


        [HttpPut("{id}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> UpdateStudent(int id, Student student)
        {

            if (id < 1 || student == null || string.IsNullOrEmpty(student.Name)
              || student.Age < 0 || student.Grade < 0 || student.Grade > 100)
            {
                return BadRequest("Invalid student data.");
            }

            var existingStudent = await _studentRepository.GetStudentByID(id);
            if (existingStudent == null)
            {
                return NotFound($"No student with id {id} was found.");
            }

            var updatedStudent=await _studentRepository.UpdateStudent(id, student);

            return Ok(updatedStudent);

        }

    }

}
