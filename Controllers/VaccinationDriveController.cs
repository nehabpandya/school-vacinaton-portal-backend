using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using school_vacinaton_portal_backend.Models;
using school_vacinaton_portal_backend.Viewmodel;
using Microsoft.EntityFrameworkCore;

namespace school_vacinaton_portal_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaccinationDriveController : Controller
    {
        private readonly SvpContext _context;
        public VaccinationDriveController(SvpContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult VaccinationDrive([FromBody] VaccinationDriveViewModel vaccinationDrive)
        {
            try
            {
                if (vaccinationDrive == null)
                {
                    return BadRequest("Student data is null.");
                }
                VaccinationDriveTbl VaccinationDriveTbl = new VaccinationDriveTbl();
                VaccinationDriveTbl.VaccineName = vaccinationDrive.VaccineName;
                VaccinationDriveTbl.Description = vaccinationDrive.Description;
                VaccinationDriveTbl.Date = vaccinationDrive.Date;
                VaccinationDriveTbl.Location = vaccinationDrive.Location;
            
                _context.Add(VaccinationDriveTbl);
                _context.SaveChanges();
                return Ok(new { message = "Student saved successfully", vaccinationDrive });
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        public async Task<IActionResult> VaccinationDrive()
        {
            try
            {
                var vaccinationDrives = await _context.VaccinationDriveTbls.ToListAsync();
                return Ok(new { message = "Vaccination Drive data fetched successfully", vaccinationDrives });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "an error occurred", error = ex.Message });
            }
        }

        // DELETE: api/Student/{studentId}
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteVaccinationDrive(int Id)
        {
            int _Id = Convert.ToInt32(Id);
            var vaccinationdrive = await _context.VaccinationDriveTbls.FirstOrDefaultAsync(s => s.VaccineId == _Id);

            if (vaccinationdrive == null)
            {
                return NotFound();
            }

            _context.VaccinationDriveTbls.Remove(vaccinationdrive);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }

       

    }
}
