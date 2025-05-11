namespace school_vacinaton_portal_backend.Viewmodel
{
    public class VaccinationRecord
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int DriveId { get; set; }
        public DateTime VaccinatedOn { get; set; } = DateTime.Now;

        // Navigation (optional)
        public StudentViewModel Student { get; set; }
        public VaccinationDriveViewModel Drive { get; set; }
    }

}
