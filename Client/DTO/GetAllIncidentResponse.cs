using Common.Contracts;

namespace Client.DTO
{
    public class GetAllIncidentResponse
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }

        public List<IncidentResponse> Items { get; set; } = new List<IncidentResponse>();
        
    }
}
