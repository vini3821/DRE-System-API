using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRESystem.API.New.Models;
using DRESystem.Data.New; // Para o DREContext
using DRESystem.Domain.New; // Para as entidades (Region, Sector, etc.)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DRESystem.API.New.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly DREContext _context;

        public RegionsController(DREContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Region>>> GetRegions()
        {
            return await _context.Regions.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Region>> GetRegion(int id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
                return NotFound();
            return region;
        }

        [HttpPost]
        public async Task<ActionResult<Region>> CreateRegion(RegionCreateDto dto)
        {
            var region = new Region { Name = dto.Name };

            _context.Regions.Add(region);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRegion), new { id = region.RegionID }, region);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegion(int id, Region region)
        {
            if (id != region.RegionID)
                return BadRequest();
            _context.Entry(region).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Regions.Any(e => e.RegionID == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
                return NotFound();

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
