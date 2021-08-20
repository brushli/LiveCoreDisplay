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
        public static string WeChatRedirectUrl = string.Empty;
        public static string WeChatAppID = string.Empty;
    }
}