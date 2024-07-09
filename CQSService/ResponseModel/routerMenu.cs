namespace CQSService.ResponseModel
{
    public class routerMenu
    {
        public long id { get; set; }

        /// <summary>
        /// 父菜單ID
        /// </summary>
        public long parentId { get; set; }

        ///// <summary>
        ///// 父結點ID路徑
        ///// </summary>
        //public string? TreePath { get; set; }

        /// <summary>
        /// 菜單名稱
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 菜單類型(1:菜單;2:目錄;3:外鏈;4:按鈕)
        /// </summary>
        public byte type { get; set; }

        /// <summary>
        /// 路由路徑(URL)
        /// </summary>
        public string? path { get; set; }

        /// <summary>
        /// 組件路徑(src/views/)
        /// </summary>
        public string? component { get; set; }

        ///// <summary>
        ///// 按鈕權限標誌
        ///// </summary>
        //public string? Perm { get; set; }

        /// <summary>
        /// 目錄只有一個子路由是否始終顯示(1:是;0:否)
        /// </summary>
        public bool alwaysShow { get; set; }

        /// <summary>
        /// 菜單是否開啟頁面緩衝(1:是;0:否)
        /// </summary>
        public bool keepAlive { get; set; }

        /// <summary>
        /// 顯示狀態(1:顯示;0:隱藏)
        /// </summary>
        public bool visible { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? sort { get; set; }

        /// <summary>
        /// 菜單圖示
        /// </summary>
        public string? icon { get; set; }

        /// <summary>
        /// 跳轉路徑
        /// </summary>
        public string? redirect { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        ///// <summary>
        ///// 路由參數(json)
        ///// </summary>
        //public string? Params { get; set; }

        public string title { get; set; }
        //public string perm { get; set; }
        public List<routerMenu> children { get; set; }
    }
}
