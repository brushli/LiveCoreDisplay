using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BJ.LiveCodeDisplay.Web.Models
{
    public class QrCodeActivityRegister
    {
        public long Id { set; get; }
        /// <summary>
        /// 活动Id
        /// </summary>
        public long PublicityId { set; get; }
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
        /// <summary>
        /// 选择地址,使用省/市/区县进行格式传递
        /// </summary>
        public string RangeAddress
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var splitDress = value.Split('/');
                    Province = splitDress.Length >= 1 ? splitDress[0]:string.Empty;
                    City = splitDress.Length >= 2 ? splitDress[1] : string.Empty;
                    Area = splitDress.Length >= 3 ? splitDress[2] : string.Empty;
                }
            }
            get
            {
                return $"{Province}/{City}/{Area}";
            }
        }
        /// <summary>
        /// 报名后是否跳转到客服码
        /// </summary>
        public bool? JumpToCustomService { set; get; }
        /// <summary>
        /// 客服码的链接，用于报名活码扫码后的跳转
        /// </summary>
        public string CustomServiceUrl { set; get; }
    }
}