using Common.Enums;

namespace API.Database.Entities
{
    public class Incident
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public IncidentSeverity Severity { get; set; } = IncidentSeverity.Low;   
        public IncidentStatus Status { get; set; } = IncidentStatus.Open;    
        public string? Location { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = default!;
    }

    
}
