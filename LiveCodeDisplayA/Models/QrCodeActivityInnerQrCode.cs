using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiveCodeDisplayA.Models
{
    public class QrCodeActivityInnerQrCode
    {
        public long Id { set; get;}
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 活码ID
        /// </summary>
        public long ActivityId { set; get; }
        /// <summary>
        /// 二维码路径,服务器路径
        /// </summary>
        public string QrCodeUrl { set; get; }
        /// <summary>
        /// 长按识别次数
        /// </summary>
        public int LongPressNum { set; get; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { set; get; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? EndTime { set; get; }
        /// <summary>
        /// 切换频率
        /// </summary>
        public int SwitchFrequency { set; get; }
        /// <summary>
        /// 状态:1、激活  2、停止访问
        /// </summary>
        public int Status { set; get; }
        /// <summary>
        /// 客户端原路径名称
        /// </summary>
        public string SourceFilePath { set; get; }
        /// <summary>
        /// 二维码底部描述内容
        /// </summary>
        public string FooderContent { set; get; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeaderImg { set; get; }
        /// <summary>
        /// 限制扫码次数
        /// </summary>
        public int? LimitNumForClientScan { set; get; }
    }
}