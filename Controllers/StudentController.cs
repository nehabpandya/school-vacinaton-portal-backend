using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_vacinaton_portal_backend.Models;
using school_vacinaton_portal_backend.Viewmodel;

namespace school_vacinaton_portal_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly SvpContext _context;
        public StudentController(SvpContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult Student([FromBody] StudentViewModel student)
        {
            try
            {
                if (student == null)
                {
                    return BadRequest("Student data is null.");
                }
                StudentsTbl studentsTbl = new StudentsTbl();
                studentsTbl.Name = student.Name;
                studentsTbl.StudentId = student.StudentId;
                studentsTbl.Class = student.Class;
                studentsTbl.DateOfBirth = student.Dob;
                studentsTbl.ParentName = student.ParentName;
                studentsTbl.ContactNumber = student.ContactNumber;
                studentsTbl.Gender = student.Gender;
                studentsTbl.Vaccinated = student.Vaccinated;
               // studentsTbl.VaccineType = student.VaccineType;
               // studentsTbl.VaccinationDate = student.VaccinationDate;
               // studentsTbl.DoseNumber = student.DoseNumber;
                studentsTbl.MedicalNote = student.MedicalNote;
                _context.Add(studentsTbl);
                _context.SaveChanges();
                return Ok(new { message = "Student saved successfully", student });
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> Student()
        {
            try
             {
                var students = await _context.StudentsTbls.ToListAsync();
                return Ok(new { message = "Student data fetched successfully", students });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        // DELETE: api/Student/{studentId}
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteStudent(int Id)
        {
            int _Id = Convert.ToInt32(Id);
            var student = await _context.StudentsTbls.FirstOrDefaultAsync(s => s.Id == _Id);

            if (student == null)
            {
                return NotFound();
            }

            _context.StudentsTbls.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadCSVFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            using var reader = new StreamReader(file.OpenReadStream());

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                BadDataFound = null,
                MissingFieldFound = null,
                HeaderValidated = null,
            };

            using var csv = new CsvReader(reader, config);

            // Tell CsvHelper the expected date format for Dob
            csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[] { "dd-MM-yyyy" };

            try
            {
                var records = csv.GetRecords<StudentViewModel>().ToList();

                if (records == null || !records.Any())
                    return BadRequest("CSV is empty or improperly formatted.");

                var students = records.Select(r => new StudentsTbl
                {
                    StudentId = r.StudentId,
                    Name = r.Name,
                    Class = r.Class,
                    DateOfBirth = r.Dob,
                    ParentName = r.ParentName,
                    ContactNumber = r.ContactNumber,
                    Gender = r.Gender,
                    MedicalNote = r.MedicalNote
                }).ToList();

                await _context.StudentsTbls.AddRangeAsync(students);
                await _context.SaveChangesAsync();

                return Ok(new { count = students.Count });
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid CSV format or error: " + ex.Message);
            }
        }



    }

}
