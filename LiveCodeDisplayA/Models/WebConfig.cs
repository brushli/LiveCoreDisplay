using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BJ.LiveCodeDisplay.Web.Models
{
    public class WebConfig
    {
        public readonly static string BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
        public readonly static string GetWeChatUserInfoUrl = BaseUrl + "api/WeChat/GetWeChatUserInfo";
        public readonly static string ScanCodeUrl = BaseUrl + "api/services/app/QrCodeActivitys/ScanCode";
        public readonly static string LongPressUrl = BaseUrl + "api/services/app/QrCodeActivityInnerCode/LongPress";
        public readonly static string CommitSignUpUrl = BaseUrl + "api/services/app/QrCodeActivityRegister/Create";
        public readonly static string SendMsgUrl = BaseUrl + "api/services/app/SmsMessageService/SendSignUpCode";
        public readonly static string GetGradeUrl = BaseUrl + "api/services/app/Grade/GetAll";
        public readonly static string GetWeChartParametersUrl = BaseUrl + "api/services/app/SystemParameters/GetWeChartParameters";
        public static string 链接处理地址 = BaseUrl + "api/TokenAuth/Link";

        public static string WeChatAppID { set; get; }
        public static string WeChatSecret { set; get; }
        public static string ScanCodeRedirectUrl { set; get; }
        /// <summary>
        /// 家长联盟首页地址
        /// </summary>
        public static string ICCCPOHomeIndexUrl { set; get; }
        /// <summary>
        /// 家长联盟地址
        /// </summary>
        public static string WeChatRedirectUrl { set; get; }
        /// <summary>
        /// 家长联盟首页完整地址
        /// </summary>
        public static string ICCCPOHomeUrl
        {
            get
            {
                return WeChatRedirectUrl + ICCCPOHomeIndexUrl;
            }
        }
    }
}