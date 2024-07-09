using CQSService.Models;
using CQSService.ResponseModel;

namespace CQSService.common
{
    public class menuFunction
    {

        public List<routerMenu> getChildrenMenu(long parentId, List<SysMenu> sysMenus)
        {
            List<routerMenu> routerMenuList = new List<routerMenu>();
            var children = sysMenus.Where(x => x.ParentId == parentId).OrderBy(x=>x.Sort).ToList();
            foreach (var item in children)
            {
                //string nameString = item.Path.Replace("-", "");
                var routerMenu = new routerMenu
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
                    title = item.Name
                };
                routerMenu.children = getChildrenMenu(item.Id, sysMenus);
                routerMenuList.Add(routerMenu);
            }
            return routerMenuList;
        }
    }
}
