using System;
using System.Collections.Generic;

namespace CQSService.Models;

public partial class SysUserRole
{
    public long Id { get; set; }

    /// <summary>
    /// 使用者ID
    /// </summary>
    public long? UserId { get; set; }

    public long? RoleId { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }
}
