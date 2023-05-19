using System;
using System.Collections.Generic;
using System.Text;

namespace BJ.LiveCodeDisplay.Web
{
    /// <summary>
    /// 链接点击
    /// </summary>
    public class LinkClickDto
    {
        public string PrimaryKey { set; get; }
        public string AuthProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderAccessCode { get; set; }
        /// <summary>
        /// 授权的APPID
        /// </summary>
        public string Appid { set; get; }
        /// <summary>
        /// APP密钥
        /// </summary>
        public string AppSecret { set; get; }
        /// <summary>
        /// 平台关联ID
        /// </summary>
        public string UnionId { set; get; }
        /// <summary>
        /// 微信用户的OPENID
        /// </summary>
        public string OpenId { set; get; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { set; get; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public string Sex { set; get; }
        /// <summary>
        /// 用户个人资料填写的省份
        /// </summary>
        public string Province { set; get; }
        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        public string City { set; get; }
        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        public string Country { set; get; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        public string HeadImgUrl { set; get; }
        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        public List<string> Privilege { set; get; }
        /// <summary>
        /// 上级用户ID，如果这个有值的时候，需要处理上下级关系
        /// </summary>
        public long? ParentUserId { set; get; }
        /// <summary>
        /// 是否拒绝
        /// </summary>
        public bool IsRefused { set; get; }
    }
}
