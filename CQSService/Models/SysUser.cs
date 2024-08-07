﻿using System;
using System.Collections.Generic;

namespace CQSService.Models;

public partial class SysUser
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long? DeptId { get; set; }

    /// <summary>
    /// 用戶狀態(1:正常;0:禁用)
    /// </summary>
    public byte Status { get; set; }

    /// <summary>
    /// 0:未刪除;1:已刪除
    /// </summary>
    public byte? Deleted { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string? Adusername { get; set; }
}
