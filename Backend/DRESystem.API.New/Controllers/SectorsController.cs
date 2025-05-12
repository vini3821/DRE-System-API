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
    public class SectorsController : ControllerBase
    {
        private readonly DREContext _context;

        public SectorsController(DREContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sector>>> GetSectors()
        {
            return await _context.Sectors.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sector>> GetSector(int id)
        {
            var sector = await _context.Sectors.FindAsync(id);
            if (sector == null)
                return NotFound();
            return sector;
        }

        [HttpPost]
        public async Task<ActionResult<Sector>> CreateSector(Sector sector)
        {
            _context.Sectors.Add(sector);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSector), new { id = sector.SectorID }, sector);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSector(int id, Sector sector)
        {
            if (id != sector.SectorID)
                return BadRequest();
            _context.Entry(sector).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Sectors.Any(e => e.SectorID == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSector(int id)
        {
            var sector = await _context.Sectors.FindAsync(id);
            if (sector == null)
                return NotFound();

            _context.Sectors.Remove(sector);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
