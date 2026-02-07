using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public record UpdateIncidentRequest
    (
        string Title,
    string? Description,
    IncidentSeverity Severity,
    IncidentStatus Status,     
    string? Location
    );
}
