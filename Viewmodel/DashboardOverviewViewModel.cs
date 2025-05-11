namespace school_vacinaton_portal_backend.Viewmodel
{
    public class DashboardOverviewViewModel
    {
        public int TotalStudents { get; set; }
        public int VaccinatedStudents { get; set; }
        public double VaccinatedPercentage { get; set; }
        public List<VaccinationDriveViewModel> UpcomingDrives { get; set; }
    }
}
