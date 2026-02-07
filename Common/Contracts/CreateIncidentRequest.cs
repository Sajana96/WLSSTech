using Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public record CreateIncidentRequest
    (
            [Required]string Title,
            string? Description,
            [Required] IncidentSeverity Severity,   
            string? Location
    );
}
