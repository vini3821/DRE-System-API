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
    public class BanksController : ControllerBase
    {
        private readonly DREContext _context;

        public BanksController(DREContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bank>>> GetBanks()
        {
            return await _context.Banks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bank>> GetBank(int id)
        {
            var bank = await _context.Banks.FindAsync(id);
            if (bank == null)
                return NotFound();
            return bank;
        }

        [HttpPost]
        public async Task<ActionResult<Bank>> CreateBank(Bank bank)
        {
            _context.Banks.Add(bank);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBank), new { id = bank.BankID }, bank);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBank(int id, Bank bank)
        {
            if (id != bank.BankID)
                return BadRequest();
            _context.Entry(bank).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Banks.Any(e => e.BankID == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBank(int id)
        {
            var bank = await _context.Banks.FindAsync(id);
            if (bank == null)
                return NotFound();

            _context.Banks.Remove(bank);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
