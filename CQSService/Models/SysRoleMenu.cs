using System;
using System.Collections.Generic;

namespace CQSService.Models;

public partial class SysRoleMenu
{
    public long Id { get; set; }

    public long RoleId { get; set; }

    public long MenuId { get; set; }
}
