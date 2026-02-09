using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public class UpdateIncidentRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IncidentSeverity? Severity { get; set; }
        public IncidentStatus? Status { get; set; }
        public string? Location { get; set; }
    }
}
