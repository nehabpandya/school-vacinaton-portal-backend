using school_vacinaton_portal_backend.Models;

namespace school_vacinaton_portal_backend.Viewmodel
{
    public class ReportsViewModel
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public List<VaccinationDriveViewModel> Vaccinations { get; set; }
    }
}
