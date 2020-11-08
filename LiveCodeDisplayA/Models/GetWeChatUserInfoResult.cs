using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiveCodeDisplayA.Models
{
    public class GetWeChatUserInfoResult
    {
        public string nickname { set; get; }
        public string sex { set; get; }
        public string province { set; get; }
        public string city { set; get; }
        public string openid { set; get; }
        public string country { set; get; }
        public string headimgurl { set; get; }
        public List<string> privilege { set; get; }
        public string unionid { set; get; }
    }
}