using System;
using System.Collections.Generic;

namespace ZealEducationManager.Entities;

public partial class Report
{
    public int ReportId { get; set; }

    public string ReportType { get; set; } = null!;

    public DateOnly GeneratedDate { get; set; }
}
