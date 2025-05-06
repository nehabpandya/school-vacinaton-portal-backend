namespace school_vacinaton_portal_backend.Viewmodel
{
    public class StudentViewModel
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public DateOnly Dob { get; set; }
        public string ParentName { get; set; }
        public string ContactNumber { get; set; }
        public string Gender { get; set; }
        public bool Vaccinated { get; set; }
        string VaccineType { get; set; }
        public int? DoseNumber { get; set; }
        public DateOnly? VaccinationDate { get; set; }
        public string MedicalNote { get; set; }
    }
}
