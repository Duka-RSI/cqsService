using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CQSService.Models;
using CQSService.ResponseModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CQSService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysUserController : ControllerBase
    {
        private readonly QuotaionContext _context;
        public SysUserController(QuotaionContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<userModel>>> GetSysUsers()
        {
            List<userModel> userModelList = new List<userModel>();
            try
            {
                var sysUserRoleView = _context.SysUserRoles.Join(_context.SysRoles,
userRole => userRole.RoleId,
role => role.Id,
(userRole, role) => new
{
UserId = userRole.UserId,
RoleName = role.Name
}
).ToList();

                foreach (SysUser item in _context.SysUsers.ToList())
                {
                    string deptName = _context.SysDepts.Any(x => x.Id == item.DeptId) ? _context.SysDepts.Where(x => x.Id == item.DeptId).ToList()[0].Name : "無部門";

                    string roleName = sysUserRoleView.Any(x => x.UserId == item.Id)? sysUserRoleView.Where(x => x.UserId == item.Id).ToList()[0].RoleName : "無角色";
                    long roleId = sysUserRoleView.Any(x => x.UserId == item.Id) ? _context.SysRoles.Where(x => x.Name == roleName).ToList()[0].Id : 0;
                    userModelList.Add(new userModel
                    {
                        Id = item.Id,
                        Username = item.Username,
                        Password = item.Password,
                        DeptId = item.DeptId,
                        deptName = deptName,
                        RoleId = roleId,
                        roleName = roleName,
                        Status = item.Status,
                        Deleted = item.Deleted,
                        CreateTime = item.CreateTime,
                        UpdateTime = item.UpdateTime,
                        Adusername = item.Adusername
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                //throw;
            }

            return userModelList;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<SysUser>> GetSysUserById(int id)
        {
            var sysUser = await _context.SysUsers.FindAsync(Convert.ToInt64(id));

            if (sysUser == null)
            {
                return NotFound();
            }

            return sysUser;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SysUser>> CreateSysUser(userModel newUserModel)
        {
            if (_context.SysUsers.Any(e => e.Username == newUserModel.Username))
            {
                return BadRequest(newUserModel.Username + " already exist.");
            }

            newUserModel.Password = common.commonFunction.Base64Encode(newUserModel.Password);
            newUserModel.CreateTime = DateTime.Now;

          SysUser newSysUser =   new SysUser
            {
                Username = newUserModel.Username,
                Password = newUserModel.Password,
                DeptId = newUserModel.DeptId,
                Status = newUserModel.Status,
                Deleted = newUserModel.Deleted,
                CreateTime = newUserModel.CreateTime,
                UpdateTime = newUserModel.UpdateTime,
                Adusername = newUserModel.Adusername
            };
            _context.SysUsers.Add(newSysUser);
            await _context.SaveChangesAsync();

            _context.SysUserRoles.Add(new SysUserRole
            {
                UserId = newSysUser.Id,
                RoleId = newUserModel.RoleId
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction("CreateSysUser", new { id = newSysUser.Id }, newUserModel);
        }



        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateSysUser(userModel updateUserModel)
        {
            if (_context.SysUsers.Any(x => x.Username == updateUserModel.Username && x.Id != updateUserModel.Id))
            {
                return BadRequest(updateUserModel.Username + " already exist.");
            }

            long id = Convert.ToInt64(updateUserModel.Id);
            if (!SysUserExists(id))
            {
                return NotFound();
            }
            //updateUserModel.Password = common.commonFunction.Base64Encode(updateUserModel.Password);
            updateUserModel.UpdateTime = DateTime.Now;

           SysUser updateSysUser =  new SysUser
            {
                Id = updateUserModel.Id,
                Username = updateUserModel.Username,
                Password = updateUserModel.Password,
                DeptId = updateUserModel.DeptId,
                Status = updateUserModel.Status,
                Deleted = updateUserModel.Deleted,
                CreateTime = updateUserModel.CreateTime,
                UpdateTime = updateUserModel.UpdateTime,
                Adusername = updateUserModel.Adusername
            };
            _context.Entry(updateSysUser).State = EntityState.Modified;


            if (_context.SysUserRoles.Any(x => x.UserId == updateSysUser.Id))
            {
                SysUserRole sysUserRole = _context.SysUserRoles.Where(x => x.UserId == updateSysUser.Id).ToList()[0];
                sysUserRole.RoleId = updateUserModel.RoleId;
                sysUserRole.UpdateTime = DateTime.Now;
                _context.Entry(sysUserRole).State = EntityState.Modified;
            }
            else
            {
                _context.SysUserRoles.Add(new SysUserRole
                {
                    UserId = updateSysUser.Id,
                    RoleId = updateUserModel.RoleId
                });
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResetSysUserPassword( int id,string password)
        {
            long userId = Convert.ToInt64(id);
            if (!SysUserExists(userId))
            {
                return NotFound();
            }
            SysUser updateSysUser = _context.SysUsers.Where(x => x.Id == userId).ToList()[0];
            updateSysUser.Password = common.commonFunction.Base64Encode(password);
            updateSysUser.UpdateTime = DateTime.Now;
            _context.Entry(updateSysUser).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        private bool SysUserExists(long id)
        {
            return _context.SysUsers.Any(e => e.Id == id);
        }

        //[HttpDelete("{id}")]
        [HttpPost("[action]")]
        public void DeleteSysUser(long id)
        {
            var sysUser = _context.SysUsers.Find(id);
            if (sysUser != null)
            {
                _context.SysUsers.Remove(sysUser);
                _context.SaveChanges();
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSysUserByLogin(string userName, string password)
        {
            string passwordEncode = common.commonFunction.Base64Encode(password);

            var sysuser = await _context.SysUsers.FirstOrDefaultAsync(x => x.Username == userName && x.Password == passwordEncode);
            if (sysuser == null)
            {
                return NotFound();
            }

            return Ok(new { name = sysuser.Username });
        }


    }
}
