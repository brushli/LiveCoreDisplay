using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BJ.LiveCodeDisplay.Web.Models
{
    public class GradeDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        /// <value>1</value>
        public int LayerLevel { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 难度选择
        /// </summary>
        public int? Difficulty { set; get; }
    }
}