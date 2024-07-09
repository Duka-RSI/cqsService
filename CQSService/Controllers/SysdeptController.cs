using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CQSService.common;
using CQSService.Models;
using CQSService.ResponseModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CQSService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysdeptController : ControllerBase
    {
        private readonly QuotaionContext _context;
        private deptFunction _deptFunction = new deptFunction();
        public SysdeptController(QuotaionContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<SysDept>>> GetSysDepts()
        {

            return await _context.SysDepts.ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSysDeptsTree()
        {
            List<deptTree> deptTree = GetDeptTrees(0);

            return Ok(deptTree);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSysDeptsTreeById(long deptId)
        {
            List<deptTree> deptTree = GetDeptTrees(deptId);

            return Ok(deptTree);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<SysDept>> GetSysDeptById(int id)
        {
            var SysDept = await _context.SysDepts.FindAsync(Convert.ToInt64(id));

            if (SysDept == null)
            {
                return NotFound();
            }

            return SysDept;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SysDept>> CreateSysDept(SysDept newSysDept)
        {
            newSysDept.CreateTime = DateTime.Now;
            _context.SysDepts.Add(newSysDept);
            await _context.SaveChangesAsync();

            return CreatedAtAction("CreateSysDept", new { id = newSysDept.Id }, newSysDept);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateSysDept(SysDept SysDept)
        {
            long id = Convert.ToInt64(SysDept.Id);
            if (!SysDeptExists(id))
            {
                return NotFound();
            }

            SysDept.UpdateTime = DateTime.Now;
            _context.Entry(SysDept).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SysDeptExists(id))
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


        //[HttpDelete("{id}")]
        [HttpPost("[action]")]
        public void DeleteSysDept(long id)
        {
            var SysDept = _context.SysDepts.Find(id);
            if (SysDept != null)
            {
                _context.SysDepts.Remove(SysDept);
                _context.SaveChanges();
            }
        }

        private bool SysDeptExists(long id)
        {
            return _context.SysDepts.Any(e => e.Id == id);
        }

        private List<deptTree> GetDeptTrees(long parentId)
        {
            List<deptTree> deptTree = new List<deptTree>();
            var firstLevelList = _context.SysDepts.Where(x => x.ParentId == parentId).OrderBy(x => x.Sort).ToList();
            var secondLevelList = _context.SysDepts.Where(x => x.ParentId != parentId).OrderBy(x => x.Sort).ToList();
            //第一層Dept
            foreach (var item in firstLevelList)
            {
                var firstLevel = new deptTree
                {
                    id = item.Id,
                    name = item.Name,
                    parentId = item.ParentId,
                    treePath = item.TreePath,
                    sort = item.Sort,
                    status = item.Status,
                    deleted = item.Deleted,
                    createTime = item.CreateTime,
                    updateTime = item.UpdateTime,
                    createBy = item.CreateBy,
                    updateBy = item.UpdateBy,
                };

                //第二層Dept
                firstLevel.children = _deptFunction.getChildrenDept(item.Id, secondLevelList);

                deptTree.Add(firstLevel);
            }

            return deptTree;
        }

    }
}
