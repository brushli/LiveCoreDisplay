using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BJ.LiveCodeDisplay.Web.Models
{
    public class GetWeChartParametersDto
    {
        public string WeChatAppID { set; get; }
        public string WeChatSecret { set; get; }
        public string ScanCodeRedirectUrl { set; get; }
        public string ICCCPOHomeIndexUrl { set; get; }
        public string WeChatRedirectUrl { set; get; }
    }
}