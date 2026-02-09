using Common.Enums;

namespace Client.DTO
{
    
        public class IncidentDto
        {
            public Guid Id { get; set; }
            public string Title { get; set; } = "";
            public string? Description { get; set; }
            public IncidentSeverity Severity { get; set; }
            public IncidentStatus Status { get; set; } 
            public string? Location { get; set; }
            public string CreatedBy { get; set; } = "";
            public DateTime CreatedAt { get; set; }
        }
    
}
