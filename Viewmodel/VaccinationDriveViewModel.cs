namespace school_vacinaton_portal_backend.Viewmodel
{
    public class VaccinationDriveViewModel
    {
        public int VaccineId { get; set; }

        public string? VaccineName { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
        public string? Location { get; set; }
    }
}
