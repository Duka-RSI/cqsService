namespace CQSService.ResponseModel
{
    public class deptTree
    {
       //id
        public long id { get; set; }

        /// <summary>
        /// 部門名稱  name
        /// </summary>
        public string name { get; set; } = null!;

        /// <summary>
        /// 父結點ID
        /// </summary>
        public long parentId { get; set; }

        /// <summary>
        /// 父結點ID路徑
        /// </summary>
        public string? treePath { get; set; }

        public int? sort { get; set; }

        /// <summary>
        /// 狀態(1:正常;0:禁用)
        /// </summary>
        public byte status { get; set; }

        /// <summary>
        /// 刪除(1:已刪除;0:未刪除)
        /// </summary>
        public byte? deleted { get; set; }

        public DateTime? createTime { get; set; }

        public DateTime? updateTime { get; set; }

        public long? createBy { get; set; }

        public long? updateBy { get; set; }

        public List<deptTree> children { get; set; }


    }
}
