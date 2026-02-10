using API.Database.DBContext;
using API.Database.Entities;
using API.Utility;
using Common.Contracts;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/incident")]
    public class IncidentController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<IncidentController> _logger;
        public IncidentController(AppDbContext db, ILogger<IncidentController> logger)
        {
            _db = db;   
            _logger = logger;
        }

        // GET api/incidents?status=Open&severity=High&search=fire&page=1&pageSize=10
        [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? status,
            [FromQuery] string? severity,
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                page = page < 1 ? 1 : page;
                pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

                var query = _db.Incidents.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(status))
                    query = query.Where(i => i.Status == (IncidentStatus)Enum.Parse(typeof(IncidentStatus), status, true));

                if (!string.IsNullOrWhiteSpace(severity))
                    query = query.Where(i => i.Severity == (IncidentSeverity)Enum.Parse(typeof(IncidentSeverity), severity, true));

                if (!string.IsNullOrWhiteSpace(search))
                    query = query.Where(i =>
                        i.Title.Contains(search) ||
                        (i.Description != null && i.Description.Contains(search)) ||
                        (i.Location != null && i.Location.Contains(search)));

                var totalCount = await query.CountAsync();

                var items = await query
                    .OrderByDescending(i => i.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(i => new IncidentResponse(
                        i.Id, i.Title, i.Description, i.Severity, i.Status,
                        i.Location, i.CreatedAt, i.CreatedBy))
                    .ToListAsync();

                return Ok(new
                {
                    page,
                    pageSize,
                    totalCount,
                    items
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured Retrieving incidents");
                return StatusCode(500, new ApiResponse() { IsSuccess = false, Message = ex.Message, HttpStatusCode = (int)HttpStatusCode.InternalServerError });
            }
            
        }

        // GET api/incidents/{id}
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var incident = await _db.Incidents.AsNoTracking()
                .Where(i => i.Id == id)
                .Select(i => new IncidentResponse(
                    i.Id, i.Title, i.Description, i.Severity, i.Status,
                    i.Location, i.CreatedAt, i.CreatedBy))
                .FirstOrDefaultAsync();

            return incident is null ? NotFound() : Ok(incident);
        }

        // POST api/incidents
        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> Create(CreateIncidentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                return BadRequest("Title is required.");

            var createdBy = UserContext.GetEmail(User);

            var incident = new Incident
            {
                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                Severity = request.Severity,
                Status = IncidentStatus.Open,
                Location = request.Location?.Trim(),
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow
            };

            _db.Incidents.Add(incident);
            await _db.SaveChangesAsync();

            var response = new IncidentResponse(
                incident.Id, incident.Title, incident.Description, incident.Severity,
                incident.Status, incident.Location, incident.CreatedAt, incident.CreatedBy);

            return CreatedAtAction(nameof(GetById), new { id = incident.Id }, response);
        }

        // PUT api/incidents/{id}
        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, UpdateIncidentRequest request)
        {
            var incident = await _db.Incidents.FirstOrDefaultAsync(i => i.Id == id);
            if (incident is null)
                return NotFound();

            if(!string.IsNullOrEmpty(request.Title))
                incident.Title = request.Title?.Trim();

            if (!string.IsNullOrEmpty(request.Description))
                incident.Description = request.Description?.Trim();

            if (request.Severity.HasValue)
                incident.Severity = request.Severity.Value;

            if (request.Status.HasValue)
                incident.Status = request.Status.Value;

            if(!string.IsNullOrEmpty(request.Location))
                incident.Location = request.Location?.Trim();

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // POST api/incidents/{id}/close
        [HttpPost("{id:guid}/close")]
        [Authorize]
        public async Task<IActionResult> Close(Guid id)
        {
            var incident = await _db.Incidents.FirstOrDefaultAsync(i => i.Id == id);
            if (incident is null)
                return NotFound();

            incident.Status = IncidentStatus.Closed;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/incidents/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")] // stricter permission
        public async Task<IActionResult> Delete(Guid id)
        {
            var incident = await _db.Incidents.FirstOrDefaultAsync(i => i.Id == id);
            if (incident is null)
                return NotFound();

            _db.Incidents.Remove(incident);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
