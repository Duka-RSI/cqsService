using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CQSService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CQSService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysRoleController : ControllerBase
    {
        private readonly QuotaionContext _context;
        public SysRoleController(QuotaionContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<SysRole>>> GetSysRoles()
        {
            return await _context.SysRoles.ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<SysRole>> GetSysRoleById(int id)
        {
            var SysRole = await _context.SysRoles.FindAsync(Convert.ToInt64(id));

            if (SysRole == null)
            {
                return NotFound();
            }

            return SysRole;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SysRole>> CreateSysRole(SysRole newSysRole)
        {
            newSysRole.CreateTime = DateTime.Now;
            _context.SysRoles.Add(newSysRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("CreateSysRole", new { id = newSysRole.Id }, newSysRole);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateSysRole(SysRole SysRole)
        {
            long id = Convert.ToInt64(SysRole.Id);
            if (!SysRoleExists(id))
            {
                return NotFound();
            }

            SysRole.UpdateTime = DateTime.Now;
            _context.Entry(SysRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SysRoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool SysRoleExists(long id)
        {
            return _context.SysRoles.Any(e => e.Id == id);
        }

        //[HttpDelete("{id}")]
        [HttpPost("[action]")]
        public void DeleteSysRole(long id)
        {
            var SysRole = _context.SysRoles.Find(id);
            if (SysRole != null)
            {
                _context.SysRoles.Remove(SysRole);
                _context.SaveChanges();
            }
        }
    }
}
