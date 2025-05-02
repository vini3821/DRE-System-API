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
    public class CostCentersController : ControllerBase
    {
        private readonly DREContext _context;

        public CostCentersController(DREContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCostCenters()
        {
            var costCenters = await _context
                .CostCenters.Include(c => c.Region)
                .Include(c => c.Sector)
                .ToListAsync();

            // Mapear para DTO para quebrar ciclos
            var result = costCenters.Select(cc => new
            {
                CCID = cc.CCID,
                Code = cc.Code,
                Description = cc.Description,
                Region = new { RegionID = cc.Region.RegionID, Name = cc.Region.Name },
                Sector = new { SectorID = cc.Sector.SectorID, Name = cc.Sector.Name },
            });

            return Ok(result);
        }

        [HttpGet("report/byregion/{regionId}")]
        public async Task<ActionResult<object>> GetDREReportByRegion(
            int regionId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate
        )
        {
            startDate ??= DateTime.Now.AddMonths(-1);
            endDate ??= DateTime.Now;

            var entries = await _context
                .Entries.Include(e => e.Collaborator)
                .ThenInclude(c => c.CostCenter)
                .ThenInclude(cc => cc.Region)
                .Where(e => e.Collaborator.CostCenter.FKRegion == regionId)
                .Where(e => e.EntryDate >= startDate && e.EntryDate <= endDate)
                .ToListAsync();

            var totalRevenue = entries.Where(e => e.EntryType == "Receita").Sum(e => e.Value);

            var totalExpenses = entries.Where(e => e.EntryType == "Despesa").Sum(e => e.Value);

            var result = new
            {
                Region = await _context.Regions.FindAsync(regionId),
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = totalRevenue,
                TotalExpenses = totalExpenses,
                NetResult = totalRevenue - totalExpenses,
                EntryDetails = entries,
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CC>> GetCostCenter(int id)
        {
            var cc = await _context
                .CostCenters.Include(c => c.Region)
                .Include(c => c.Sector)
                .FirstOrDefaultAsync(c => c.CCID == id);

            if (cc == null)
                return NotFound();
            return cc;
        }

        [HttpPost]
        public async Task<ActionResult<CC>> CreateCostCenter(CC cc)
        {
            _context.CostCenters.Add(cc);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCostCenter), new { id = cc.CCID }, cc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCostCenter(int id, CC cc)
        {
            if (id != cc.CCID)
                return BadRequest();
            _context.Entry(cc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.CostCenters.Any(e => e.CCID == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCostCenter(int id)
        {
            var cc = await _context.CostCenters.FindAsync(id);
            if (cc == null)
                return NotFound();

            _context.CostCenters.Remove(cc);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
