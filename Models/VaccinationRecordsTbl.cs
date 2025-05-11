using System;
using System.Collections.Generic;

namespace school_vacinaton_portal_backend.Models;

public partial class VaccinationRecordsTbl
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int DriveId { get; set; }

    public DateTime VaccinatedOn { get; set; }

    public virtual VaccinationDriveTbl Drive { get; set; } = null!;

    public virtual StudentsTbl Student { get; set; } = null!;
}
