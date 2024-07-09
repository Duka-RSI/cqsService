using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using CQSService.common;
using CQSService.Models;
using CQSService.ResponseModel;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CQSService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysMenuController : ControllerBase
    {
        private readonly QuotaionContext _context;
        private menuFunction _menuFunction = new menuFunction();
        public SysMenuController(QuotaionContext context)
        {
            _context = context;
        }

        // GET: api/<SysMenuController>
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<SysMenu>>> GetSysMenus()
        {

            return await _context.SysMenus.ToListAsync();
        }

        // GET api/<SysMenuController>/5
        //[HttpGet("{id}")]
        [HttpGet("[action]")]
        public async Task<ActionResult<SysMenu>> GetSysMenuById(int id)
        {
            var sysMenu = await _context.SysMenus.FindAsync(id);

            if (sysMenu == null)
            {
                return NotFound();
            }

            return sysMenu;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SysMenu>> CreateSysMenu(SysMenu newSysMenu)
        {
            newSysMenu.CreateTime = DateTime.Now;
            _context.SysMenus.Add(newSysMenu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("CreateSysMenu", new { id = newSysMenu.Id }, newSysMenu);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateSysMenu(SysMenu sysMenu)
        {
            long id = Convert.ToInt64(sysMenu.Id);
            if (!SysMenuExists(id))
            {
                return NotFound();
            }

            sysMenu.UpdateTime = DateTime.Now;
            _context.Entry(sysMenu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SysMenuExists(id))
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

        private bool SysMenuExists(long id)
        {
            return _context.SysMenus.Any(e => e.Id == id);
        }

        //// POST api/<SysMenuController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<SysMenuController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<SysMenuController>/5
        //[HttpDelete("{id}")]
        [HttpPost("[action]")]
        public void DeleteSysMenu(long id)
        {
            var sysMenu = _context.SysMenus.Find(id);
            if (sysMenu != null)
            {
                _context.SysMenus.Remove(sysMenu);
                _context.SaveChanges();
            }
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetSysMenusByUserName(string userName)
        {
            var sysuser = await _context.SysUsers.FirstOrDefaultAsync(x => x.Username == userName);
            if (sysuser == null)
            {
                return NotFound();
            }
            var userRoles = await _context.SysUserRoles.Where(x => x.UserId == sysuser.Id).ToListAsync();
            if (userRoles == null)
            {
                return NotFound();
            }

            //檢查是否包含有Admin角色,如果有就取得所有的menu
            if (userRoles.Join(_context.SysRoles,userRole => userRole.RoleId,role => role.Id,(userRole, role)  => new{ 
                    UserId = userRole.UserId,
                    RoleCode = role.Code
                }).Any(x => x.RoleCode.ToUpper() == "ADMIN"))
            {
            return Ok(GetMenusTree(await _context.SysMenus.ToListAsync()));
            }


            var sysRoleMenus = await _context.SysRoleMenus.Where(x => userRoles.Select(x => x.RoleId).Contains(x.RoleId)).ToListAsync();
            if (sysRoleMenus == null)
            {
                return NotFound();
            }
            //取得所屬Role所有的menu
            var sysMenus = await _context.SysMenus.Where(x => sysRoleMenus.Select(x => x.MenuId).Contains(x.Id)).ToListAsync();
            if (sysMenus == null)
            {
                return NotFound();
            }

          return Ok(GetMenusTree(sysMenus));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSysMenusIDByRoleId(long roleId)
        {
            var sysRoleMenus = await _context.SysRoleMenus.Where(x =>x.RoleId == roleId).ToListAsync();
            if (sysRoleMenus == null)
            {
                return NotFound();
            }
            //取得所屬Role所有的menu
            var sysMenus = await _context.SysMenus.Where(x => sysRoleMenus.Select(x => x.MenuId).Contains(x.Id)).Select(x=>x.Id).ToListAsync();
            if (sysMenus == null)
            {
                return NotFound();
            }

            return Ok(sysMenus);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateSysRoleMenus(long roleId, long[] menusId) 
        {
            try
            {
                //找出要刪除的
                var deleteRoleMenu = await _context.SysRoleMenus.Where(x => x.RoleId == roleId && !menusId.Contains(x.MenuId)).Select(x => x.Id).ToListAsync();

                //找出要新增的
                var addRoleMenu = menusId.Where(x => !_context.SysRoleMenus.Any(y => y.RoleId == roleId && y.MenuId == x)).Select(x => new SysRoleMenu { RoleId = roleId, MenuId = x }).ToList();

                //刪除
                foreach (var item in deleteRoleMenu)
                {
                    var sysRoleMenu = await _context.SysRoleMenus.FindAsync(item);
                    if (sysRoleMenu != null)
                    {
                        _context.SysRoleMenus.Remove(sysRoleMenu);
                    }
                }

                foreach (var item in addRoleMenu)
                {
                    _context.SysRoleMenus.Add(item);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();           

        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetSysMenusTree()
        {
          
            var sysMenus = await _context.SysMenus.ToListAsync();
            if (sysMenus == null)
            {
                return NotFound();
            }
            return Ok(GetMenusTree(sysMenus));
        }


        private List<routerMenu> GetMenusTree(List<SysMenu> sysMenus)
        {

            List<routerMenu> routerMenuList = new List<routerMenu>();

            //先取得所有的一級menu
            var firstLevelMenu = sysMenus.Where(x => x.Type == 2).OrderBy(x => x.Sort).ToList();
            //menu
            var menuList = sysMenus.Where(x => x.Type == 1).ToList();
            foreach (var item in firstLevelMenu)
            {

                //string nameString = item.Path.Replace("-", "");
                var firstLevel = new routerMenu
                {
                    id = item.Id,
                    parentId = item.ParentId,
                    name = item.Name,
                    type = item.Type,
                    path = item.Path,
                    component = item.Component,
                    alwaysShow = item.AlwaysShow == 1 ? true : false,
                    keepAlive = item.KeepAlive == 1 ? true : false,
                    visible = item.Visible == 1 ? true : false,
                    sort = item.Sort,
                    icon = item.Icon,
                    redirect = item.Redirect,
                    CreateTime = item.CreateTime,
                    UpdateTime = item.UpdateTime,
                    title = item.Name,
                };

                //取得第一層menu的子menu
                firstLevel.children = _menuFunction.getChildrenMenu(item.Id, menuList);
                routerMenuList.Add(firstLevel);
            }
            return routerMenuList;
        }


    }
}
