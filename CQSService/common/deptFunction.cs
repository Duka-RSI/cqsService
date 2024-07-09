using CQSService.Models;
using CQSService.ResponseModel;

namespace CQSService.common
{
    public class deptFunction
    {

        public List<deptTree> getChildrenDept(long parentId, List<SysDept> sysDepts)
        {
            List<deptTree> deptTree = new List<deptTree>();
            var children = sysDepts.Where(x => x.ParentId == parentId).OrderBy(x=>x.Sort).ToList();
            foreach (var item in children)
            {
                var dept = new deptTree
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
                dept.children = getChildrenDept(item.Id, sysDepts);
                deptTree.Add(dept);
            }
            return deptTree;
        }
    }
}
