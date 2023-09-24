using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class EmployeeDetail
{
    public long Id { get; set; }

    public string? UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? DisplayName { get; set; }

    public string Email { get; set; } = null!;

    public long ContactNumber { get; set; }

    public string? Address { get; set; }

    public DateTime? CreatedDate { get; set; }

    public long? CreatedBy { get; set; }

    public bool? Status { get; set; }

    public virtual AspNetUser? User { get; set; }
}
