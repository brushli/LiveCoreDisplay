using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BJ.LiveCodeDisplay.Web.Models
{
    public class QrCodeActivityRegister
    {
        public long Id { set; get; }
        public long ActivityId { set; get; }
        /// <summary>
        /// 活码推广人
        /// </summary>
        public long UserId { set; get; }
        /// <summary>
        /// 活码所属人
        /// </summary>
        public long OwnerUserId { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string OpenId { set; get; }
        /// <summary>
        /// 报名所属注册所属人ID
        /// </summary>
        public long RegistrationUserId { set; get; }
        /// <summary>
        /// 报名所属注册所属人名称
        /// </summary>
        public string RegistrationUserName { set; get; }
        public string Name { set; get; }
        public string Grade { set; get; }
        public string Phone { set; get; }
        public string School { set; get; }
        public string Email { set; get; }
        public string Province { set; get; }
        public string City { set; get; }
        public string Area { set; get; }
        public string County { set; get; }
        public string Town { set; get; }
        public string Address { set; get; }
        public string IDCard { set; get; }
        public string LevelOfDescription { set; get; }
        public string ClassType { set; get; }
        public string ChooseTime { set; get; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string SmsCode { set; get; }
    }
}