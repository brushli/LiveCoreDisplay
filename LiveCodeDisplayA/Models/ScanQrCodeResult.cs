using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BJ.LiveCodeDisplay.Web.Models
{
    public class ScanQrCodeResult
    {
        /// <summary>
        /// 业务是否正常执行成功
        /// </summary>
        public bool IsSucess { set; get; }
        /// <summary>
        /// 错误或成功的具体消息
        /// </summary>
        public string Message { set; get; }
        /// <summary>
        /// 存储关键数据
        /// </summary>
        public string Data { set; get; }
        /// <summary>
        /// 返回编码
        /// </summary>
        public int ResultCode { set; get; }
        /// <summary>
        /// 获取的码
        /// </summary>
        public QrCodeActivityInnerQrCode InnerCode { set; get; }
        public QrCodeActivitys QrCodeActivity { set; get; }
    }
}