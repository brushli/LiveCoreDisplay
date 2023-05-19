using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BJ.LiveCodeDisplay.Web.Models
{
    public class LinkClickResult:ResultDto
    {
        public string LinkContent { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public bool WaitingForActivation { get; set; }
        public long UserId { set; get; }
        /// <summary>
        /// 是否首次点击
        /// </summary>
        public bool IsFirst { set; get; }
        public int LinkType { set; get; }
    }
}