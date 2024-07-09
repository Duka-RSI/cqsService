using System;
using System.Collections.Generic;

namespace CQSService.Models;

public partial class SysRole
{
    public long Id { get; set; }

    /// <summary>
    /// 角色名稱
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 角色編碼
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 顯示順序
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// 角色狀態(1:正常;0:停用)
    /// </summary>
    public byte? Status { get; set; }

    /// <summary>
    /// 數據權限(0:所有數據;1:部門及子部門;2:本部門;3:本人)
    /// </summary>
    public byte? DataScope { get; set; }

    /// <summary>
    /// 刪除(0:未刪除;1:已刪除)
    /// </summary>
    public byte Deleted { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }
}
