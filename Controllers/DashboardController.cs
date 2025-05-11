using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_vacinaton_portal_backend.Models;
using school_vacinaton_portal_backend.Viewmodel;

namespace school_vacinaton_portal_backend.Controllers
{
    public class DashboardController : Controller
    {
        private readonly SvpContext _context;

        public DashboardController(SvpContext context)
        {
            _context = context;
        }

        [HttpGet("api/overview")]
        public async Task<IActionResult> GetDashboardOverview()
        {
            var totalStudents = await _context.StudentsTbls.CountAsync();

            // Count students who have at least one vaccination record
            var vaccinatedStudents = await _context.VaccinationRecordsTbls
                .Select(v => v.StudentId)
                .Distinct()
                .CountAsync();

            var today = DateOnly.FromDateTime(DateTime.Today);
            var endDate = today.AddDays(30);

            var upcomingDrives = await _context.VaccinationDriveTbls
                .Where(d => d.Date >= today && d.Date <= endDate)
                .Select(d => new VaccinationDriveViewModel
                {
                    VaccineId = d.VaccineId,
                    VaccineName = d.VaccineName,
                    Date = d.Date
                })
                .ToListAsync();


            var result = new DashboardOverviewViewModel
            {
                TotalStudents = totalStudents,
                VaccinatedStudents = vaccinatedStudents,
                VaccinatedPercentage = totalStudents == 0 ? 0: Math.Round((double)vaccinatedStudents / totalStudents * 100, 2)  ,
                UpcomingDrives = upcomingDrives
            };

            return Ok(result);
        }
    }
}
