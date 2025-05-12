using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRESystem.Data.New; // Para o DREContext
using DRESystem.Domain.New; // Para as entidades (Region, Sector, etc.)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DRESystem.API.New.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollaboratorsController : ControllerBase
    {
        private readonly DREContext _context;

        public CollaboratorsController(DREContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCollaborators()
        {
            var collaborators = await _context
                .Collaborators.Include(c => c.CostCenter)
                .ThenInclude(cc => cc.Region)
                .Include(c => c.CostCenter)
                .ThenInclude(cc => cc.Sector)
                .ToListAsync();

            // Mapear para DTO para quebrar ciclos
            var result = collaborators.Select(c => new
            {
                CollaboratorID = c.CollaboratorID,
                Name = c.Name,
                FKCC = c.FKCC,
                CostCenter = new
                {
                    CCID = c.CostCenter.CCID,
                    Code = c.CostCenter.Code,
                    Description = c.CostCenter.Description,
                    Region = new
                    {
                        RegionID = c.CostCenter.Region.RegionID,
                        Name = c.CostCenter.Region.Name,
                    },
                    Sector = new
                    {
                        SectorID = c.CostCenter.Sector.SectorID,
                        Name = c.CostCenter.Sector.Name,
                    },
                },
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCollaborator(int id)
        {
            var collaborator = await _context
                .Collaborators.Include(c => c.CostCenter)
                .ThenInclude(cc => cc.Region)
                .Include(c => c.CostCenter)
                .ThenInclude(cc => cc.Sector)
                .FirstOrDefaultAsync(c => c.CollaboratorID == id);

            if (collaborator == null)
                return NotFound();

            // Mapear para DTO para quebrar ciclos
            var result = new
            {
                CollaboratorID = collaborator.CollaboratorID,
                Name = collaborator.Name,
                FKCC = collaborator.FKCC,
                CostCenter = new
                {
                    CCID = collaborator.CostCenter.CCID,
                    Code = collaborator.CostCenter.Code,
                    Description = collaborator.CostCenter.Description,
                    Region = new
                    {
                        RegionID = collaborator.CostCenter.Region.RegionID,
                        Name = collaborator.CostCenter.Region.Name,
                    },
                    Sector = new
                    {
                        SectorID = collaborator.CostCenter.Sector.SectorID,
                        Name = collaborator.CostCenter.Sector.Name,
                    },
                },
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Collaborator>> CreateCollaborator(Collaborator collaborator)
        {
            _context.Collaborators.Add(collaborator);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetCollaborator),
                new { id = collaborator.CollaboratorID },
                collaborator
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCollaborator(int id, Collaborator collaborator)
        {
            if (id != collaborator.CollaboratorID)
                return BadRequest();
            _context.Entry(collaborator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Collaborators.Any(e => e.CollaboratorID == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollaborator(int id)
        {
            var collaborator = await _context.Collaborators.FindAsync(id);
            if (collaborator == null)
                return NotFound();

            _context.Collaborators.Remove(collaborator);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
