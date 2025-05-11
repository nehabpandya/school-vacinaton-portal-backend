using System;
using System.Collections.Generic;

namespace school_vacinaton_portal_backend.Models;

public partial class StudentsTbl
{
    public int Id { get; set; }

    public string? StudentId { get; set; }

    public string Name { get; set; } = null!;

    public string Class { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string ParentName { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateOnly? VaccinationDate { get; set; }

    public string? MedicalNote { get; set; }

    public DateTime? timestamp { get; set; }

    public virtual ICollection<VaccinationRecordsTbl> VaccinationRecordsTbls { get; set; } = new List<VaccinationRecordsTbl>();
}
