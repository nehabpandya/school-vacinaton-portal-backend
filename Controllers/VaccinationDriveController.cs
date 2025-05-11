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
        [HttpPost]
        [HttpPost]
        public IActionResult VaccinationDrive([FromBody] VaccinationDriveViewModel vaccinationDrive)
        {
            try
            {
                if (vaccinationDrive == null)
                {
                    return BadRequest("Vaccination Drive data is null.");
                }

                // 1. Enforce 15-day advance scheduling
                var today = DateOnly.FromDateTime(DateTime.Today); // Convert DateTime to DateOnly
                if (vaccinationDrive.Date < today.AddDays(15))
                {
                    return BadRequest("Vaccination drive must be scheduled at least 15 days in advance.");
                }

                // 2. Prevent overlapping drives (same date and location)
                bool conflictExists = _context.VaccinationDriveTbls.Any(d =>
                   d.Date == vaccinationDrive.Date &&  // Compare DateOnly with DateOnly directly
                   d.Location.ToLower() == vaccinationDrive.Location.ToLower()
               );

                if (conflictExists)
                {
                    return BadRequest("A vaccination drive is already scheduled at this location on this date.");
                }

                // 3. Save the drive
                var driveEntity = new VaccinationDriveTbl
                {
                    VaccineName = vaccinationDrive.VaccineName,
                    Description = vaccinationDrive.Description,
                    Date = vaccinationDrive.Date,  // Convert DateOnly to DateTime for storage
                    Location = vaccinationDrive.Location,
                    NoOfDoseAvl = vaccinationDrive.Dose,
                    timestamp = DateTime.Now
                };

                _context.Add(driveEntity);
                _context.SaveChanges();

                return Ok(new { message = "Vaccination Drive saved successfully", vaccinationDrive });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> VaccinationDrive()
        {
            try
            {
                var vaccinationDrives = await _context.VaccinationDriveTbls
                    .Select(drive => new VaccinationDriveViewModel
                    {
                        VaccineId = drive.VaccineId,
                        VaccineName = drive.VaccineName,
                        Description = drive.Description,
                        Date = drive.Date,  // Adjust if Date is not in DateOnly format
                        Location = drive.Location,
                        Dose = drive.NoOfDoseAvl
                    })
                    .ToListAsync();


                return Ok(new { message = "Vaccination Drive data fetched successfully", vaccinationDrives });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "an error occurred", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVaccinationDrive(int id, [FromBody] VaccinationDriveViewModel updatedDrive)
        {
            if (id != updatedDrive.VaccineId)
            {
                return BadRequest("Vaccine ID mismatch.");
            }

            var existingDrive = await _context.VaccinationDriveTbls.FindAsync(id);
            if (existingDrive == null)
            {
                return NotFound("Vaccination drive not found.");
            }

            // 1. Enforce 15-day advance scheduling
            var today = DateOnly.FromDateTime(DateTime.Today); // Convert DateTime to DateOnly
            if (updatedDrive.Date < today.AddDays(15))
            {
                return BadRequest("Vaccination drive must be scheduled at least 15 days in advance.");
            }

            // 2. Prevent overlapping drives (same date and location)
            bool conflictExists = _context.VaccinationDriveTbls.Any(d =>
                d.Date == updatedDrive.Date &&  // Compare DateOnly with DateOnly directly
                d.Location.ToLower() == updatedDrive.Location.ToLower() &&
                d.VaccineId != id  // Exclude the current drive being updated
            );

            if (conflictExists)
            {
                return BadRequest("A vaccination drive is already scheduled at this location on this date.");
            }

            // Update fields
            existingDrive.VaccineName = updatedDrive.VaccineName;
            existingDrive.Description = updatedDrive.Description;
            existingDrive.Date = updatedDrive.Date;
            existingDrive.Location = updatedDrive.Location;

            await _context.SaveChangesAsync();

            return Ok(existingDrive);
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


        [HttpPost("mark")]
        public async Task<IActionResult> MarkVaccinated([FromBody] VaccinationRecordsTbls dto)
        {
            try
            {
                // Check if already vaccinated
                bool alreadymarked = await _context.VaccinationRecordsTbls
                    .AnyAsync(v => v.StudentId == dto.StudentId && v.DriveId == dto.DriveId);

                if (alreadymarked)
                {
                    return BadRequest("student already vaccinated for this drive.");
                }

                // Map ViewModel to Model
                VaccinationRecordsTbl record = new VaccinationRecordsTbl
                {
                    StudentId = dto.StudentId,
                    DriveId = dto.DriveId
                };

                _context.VaccinationRecordsTbls.Add(record);
                await _context.SaveChangesAsync();

                return Ok(record);
            }
            catch (Exception)
            {
                return BadRequest("Not found student or drive.");
                throw;
            }
            
        }


        [HttpGet]
        [Route("stdvaccinationrecords")]
        public async Task<IActionResult> GetVaccinationRecords()
        {
            try
            {
                // First, fetch necessary data from individual tables
                var students = await _context.StudentsTbls.ToListAsync();
                var drives = await _context.VaccinationDriveTbls.ToListAsync();
                var records = await _context.VaccinationRecordsTbls.ToListAsync();

                // Join manually using LINQ
                var result = records.Select(r =>
                {
                    var student = students.FirstOrDefault(s => s.Id == r.StudentId);
                    var drive = drives.FirstOrDefault(d => d.VaccineId == r.DriveId);

                    return new
                    {
                        r.Id,
                        r.StudentId,
                        StudentName = student?.Name,
                        r.DriveId,
                        DriveName = drive?.VaccineName,
                        DriveDate = drive?.Date
                    };
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetVaccinationRecords: " + ex.Message);
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }


    }
}
