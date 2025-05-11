using Microsoft.AspNetCore.Mvc;
using school_vacinaton_portal_backend.Models;
using school_vacinaton_portal_backend.Viewmodel;

namespace school_vacinaton_portal_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViewReportsController : Controller
    {
        private readonly SvpContext _context;
        public ViewReportsController(SvpContext context)
        {
            _context = context;
        }
        [HttpGet("student-vaccinations")]
        public IActionResult GetStudentVaccinationData()
        {
            var data = _context.StudentsTbls
                .Select(student => new ReportsViewModel
                {
                    StudentId = student.StudentId,
                    StudentName = student.Name,
                    Vaccinations = _context.VaccinationRecordsTbls
                        .Where(r => r.StudentId == student.Id)
                        .Join(_context.VaccinationDriveTbls,
                              record => record.DriveId,
                              drive => drive.VaccineId,
                              (record, drive) => new VaccinationDriveViewModel
                              {
                                  VaccineName = drive.VaccineName,
                                  Date = drive.Date,
                                  Location = drive.Location
                              })
                        .ToList()
                })
                .ToList();

            return Ok(data);
        }

    }
}
