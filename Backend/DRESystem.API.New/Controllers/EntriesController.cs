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
    public class EntriesController : ControllerBase
    {
        private readonly DREContext _context;

        public EntriesController(DREContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entry>>> GetEntries()
        {
            try
            {
                // Simplificar a consulta e evitar includes redundantes
                var entries = await _context
                    .Entries.Include(e => e.Collaborator)
                    .ThenInclude(c => c.CostCenter)
                    .ThenInclude(cc => cc.Region)
                    .Include(e => e.Collaborator.CostCenter.Sector) // Mais eficiente que duplicate ThenIncludes
                    .Include(e => e.Bank)
                    .ToListAsync();

                return Ok(entries); // Retornar com Ok() explícito para garantir o status 200
            }
            catch (Exception ex)
            {
                // Log do erro para diagnóstico
                Console.WriteLine($"Erro ao recuperar entries: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                // Retornar mensagem amigável para o cliente
                return StatusCode(
                    500,
                    new { message = "Erro ao buscar lançamentos", error = ex.Message }
                );
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Entry>> GetEntry(int id)
        {
            var entry = await _context
                .Entries.Include(e => e.Collaborator)
                .ThenInclude(c => c.CostCenter)
                .ThenInclude(cc => cc.Region)
                .Include(e => e.Collaborator)
                .ThenInclude(c => c.CostCenter)
                .ThenInclude(cc => cc.Sector)
                .Include(e => e.Bank)
                .FirstOrDefaultAsync(e => e.EntryID == id);

            if (entry == null)
                return NotFound();
            return entry;
        }

        [HttpGet("byregion/{regionId}")]
        public async Task<ActionResult<IEnumerable<Entry>>> GetEntriesByRegion(int regionId)
        {
            var entries = await _context
                .Entries.Include(e => e.Collaborator)
                .ThenInclude(c => c.CostCenter)
                .ThenInclude(cc => cc.Region)
                .Include(e => e.Bank)
                .Where(e => e.Collaborator.CostCenter.FKRegion == regionId)
                .ToListAsync();

            return entries;
        }

        [HttpGet("bysector/{sectorId}")]
        public async Task<ActionResult<IEnumerable<Entry>>> GetEntriesBySector(int sectorId)
        {
            var entries = await _context
                .Entries.Include(e => e.Collaborator)
                .ThenInclude(c => c.CostCenter)
                .ThenInclude(cc => cc.Sector)
                .Include(e => e.Bank)
                .Where(e => e.Collaborator.CostCenter.FKSector == sectorId)
                .ToListAsync();

            return entries;
        }

        [HttpPost]
        public async Task<ActionResult<Entry>> CreateEntry(Entry entry)
        {
            _context.Entries.Add(entry);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEntry), new { id = entry.EntryID }, entry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntry(int id, Entry entry)
        {
            if (id != entry.EntryID)
                return BadRequest();
            _context.Entry(entry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Entries.Any(e => e.EntryID == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            var entry = await _context.Entries.FindAsync(id);
            if (entry == null)
                return NotFound();

            _context.Entries.Remove(entry);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
