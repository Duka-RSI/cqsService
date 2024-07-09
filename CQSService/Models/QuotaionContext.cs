using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CQSService.Models;

public partial class QuotaionContext : DbContext
{
    public QuotaionContext()
    {
    }

    public QuotaionContext(DbContextOptions<QuotaionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SysDept> SysDepts { get; set; }

    public virtual DbSet<SysMenu> SysMenus { get; set; }

    public virtual DbSet<SysRole> SysRoles { get; set; }

    public virtual DbSet<SysRoleMenu> SysRoleMenus { get; set; }

    public virtual DbSet<SysUser> SysUsers { get; set; }

    public virtual DbSet<SysUserRole> SysUserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SysDept>(entity =>
        {
            entity.ToTable("sys_dept");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy).HasColumnName("create_by");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.Deleted)
                .HasDefaultValueSql("((0))")
                .HasComment("刪除(1:已刪除;0:未刪除)")
                .HasColumnName("deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasComment("部門名稱")
                .HasColumnName("name");
            entity.Property(e => e.ParentId)
                .HasComment("父結點ID")
                .HasColumnName("parent_id");
            entity.Property(e => e.Sort)
                .HasDefaultValueSql("((0))")
                .HasColumnName("sort");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasComment("狀態(1:正常;0:禁用)")
                .HasColumnName("status");
            entity.Property(e => e.TreePath)
                .HasMaxLength(255)
                .HasComment("父結點ID路徑")
                .HasColumnName("tree_path");
            entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<SysMenu>(entity =>
        {
            entity.ToTable("sys_menu");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AlwaysShow)
                .HasComment("目錄只有一個子路由是否始終顯示(1:是;0:否)")
                .HasColumnName("always_show");
            entity.Property(e => e.Component)
                .HasMaxLength(250)
                .HasComment("組件路徑(src/views/)")
                .HasColumnName("component");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.Icon)
                .HasMaxLength(100)
                .HasDefaultValueSql("('')")
                .HasComment("菜單圖示")
                .HasColumnName("icon");
            entity.Property(e => e.KeepAlive)
                .HasComment("菜單是否開啟頁面緩衝(1:是;0:否)")
                .HasColumnName("keep_alive");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasComment("菜單名稱")
                .HasColumnName("name");
            entity.Property(e => e.Params)
                .HasComment("路由參數(json)")
                .HasColumnName("params");
            entity.Property(e => e.ParentId)
                .HasComment("父菜單ID")
                .HasColumnName("parent_id");
            entity.Property(e => e.Path)
                .HasMaxLength(250)
                .HasDefaultValueSql("(' ')")
                .HasComment("路由路徑(URL)")
                .HasColumnName("path");
            entity.Property(e => e.Perm)
                .HasMaxLength(250)
                .HasComment("按鈕權限標誌")
                .HasColumnName("perm");
            entity.Property(e => e.Redirect)
                .HasMaxLength(150)
                .HasComment("跳轉路徑")
                .HasColumnName("redirect");
            entity.Property(e => e.Sort)
                .HasDefaultValueSql("((0))")
                .HasComment("排序")
                .HasColumnName("sort");
            entity.Property(e => e.TreePath)
                .HasMaxLength(255)
                .HasComment("父結點ID路徑")
                .HasColumnName("tree_path");
            entity.Property(e => e.Type)
                .HasComment("菜單類型(1:菜單;2:目錄;3:外鏈;4:按鈕)")
                .HasColumnName("type");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time");
            entity.Property(e => e.Visible)
                .HasDefaultValueSql("((1))")
                .HasComment("顯示狀態(1:顯示;0:隱藏)")
                .HasColumnName("visible");
        });

        modelBuilder.Entity<SysRole>(entity =>
        {
            entity.ToTable("sys_role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasComment("角色編碼")
                .HasColumnName("code");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.DataScope)
                .HasComment("數據權限(0:所有數據;1:部門及子部門;2:本部門;3:本人)")
                .HasColumnName("data_scope");
            entity.Property(e => e.Deleted)
                .HasComment("刪除(0:未刪除;1:已刪除)")
                .HasColumnName("deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasDefaultValueSql("(' ')")
                .HasComment("角色名稱")
                .HasColumnName("name");
            entity.Property(e => e.Sort)
                .HasComment("顯示順序")
                .HasColumnName("sort");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasComment("角色狀態(1:正常;0:停用)")
                .HasColumnName("status");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<SysRoleMenu>(entity =>
        {
            entity.ToTable("sys_role_menu");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MenuId).HasColumnName("menu_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
        });

        modelBuilder.Entity<SysUser>(entity =>
        {
            entity.ToTable("sys_user");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adusername)
                .HasMaxLength(100)
                .HasDefaultValueSql("('')")
                .HasColumnName("ADusername");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.Deleted)
                .HasDefaultValueSql("((0))")
                .HasComment("0:未刪除;1:已刪除")
                .HasColumnName("deleted");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasComment("用戶狀態(1:正常;0:禁用)")
                .HasColumnName("status");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<SysUserRole>(entity =>
        {
            entity.ToTable("sys_user_role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId)
                .HasComment("使用者ID")
                .HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
