using System;
using System.Collections.Generic;

namespace CQSService.Models;

public partial class SysDept
{
    public long Id { get; set; }

    /// <summary>
    /// 部門名稱
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 父結點ID
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 父結點ID路徑
    /// </summary>
    public string? TreePath { get; set; }

    public int? Sort { get; set; }

    /// <summary>
    /// 狀態(1:正常;0:禁用)
    /// </summary>
    public byte Status { get; set; }

    /// <summary>
    /// 刪除(1:已刪除;0:未刪除)
    /// </summary>
    public byte? Deleted { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public long? CreateBy { get; set; }

    public long? UpdateBy { get; set; }
}
