using System;
using System.Collections.Generic;

namespace school_vacinaton_portal_backend.Models;

public partial class VaccinationDriveTbl
{
    public int VaccineId { get; set; }

    public string VaccineName { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly Date { get; set; }

    public string Location { get; set; } = null!;

    public int? NoOfDoseAvl { get; set; }

    public DateTime? timestamp { get; set; }

    public virtual ICollection<VaccinationRecordsTbl> VaccinationRecordsTbls { get; set; } = new List<VaccinationRecordsTbl>();
}
