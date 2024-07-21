using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab7.Data;
using Lab7.Models;

namespace Lab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentDbContext _context;

        public StudentsController(StudentDbContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // returned when we return list of Students successfully
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // returned when there is an error in processing the request
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            try
            {
                return Ok(await _context.Students.ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // returned when the student is found and returned successfully
        [ProducesResponseType(StatusCodes.Status404NotFound)] // returned when the student with the specified ID is not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // returned when there is an error in processing the request
        public async Task<ActionResult<Student>> GetStudent(Guid id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);

                if (student == null)
                {
                    return NotFound();
                }

                return Ok(student);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // returned when the student is updated successfully
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // returned when the student ID in the URL doesn't match the student ID in the body
        [ProducesResponseType(StatusCodes.Status404NotFound)] // returned when the student with the specified ID is not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // returned when there is an error in processing the request
        public async Task<IActionResult> PutStudent(Guid id, Student student)
        {
            if (id != student.ID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(student);
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] // returned when the student is created successfully
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // returned when the student data is invalid
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // returned when there is an error in processing the request
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetStudent", new { id = student.ID }, student);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // returned when the student is deleted successfully
        [ProducesResponseType(StatusCodes.Status404NotFound)] // returned when the student with the specified ID is not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // returned when there is an error in processing the request
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    return NotFound();
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();

                return Ok(student);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
