using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiveCodeDisplayA.Models
{
    public class QrCodeActivitys
    {
        public long Id { set; get; }
        /// <summary>
        /// 活码名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 虚拟ID
        /// </summary>
        public string VisualId { set; get; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { set; get; }
        /// <summary>
        /// 有效期：开始
        /// </summary>
        public DateTime? BeginTime { set; get; }
        /// <summary>
        /// 有效期：结束
        /// </summary>
        public DateTime? EndTime { set; get; }
        /// <summary>
        /// 状态:1、激活  2、停止访问
        /// </summary>
        public int Status { set; get; }
        /// <summary>
        /// 活码类型：群码、客服
        /// </summary>
        public string QrType { set; get; }
        /// <summary>
        /// 限制扫码次数
        /// </summary>
        public int? LimitNumForClientScan { set; get; }
        /// <summary>
        /// 总访问人数
        /// </summary>
        public int TotalVisitors { set; get; }
        /// <summary>
        /// 总长按识别人数
        /// </summary>
        public int TotalLongPressNum { set; get; }
        /// <summary>
        /// 已长按识别人数
        /// </summary>
        public int LongPressNumed { set; get; }
        /// <summary>
        /// 活码地址，目的是为了不向外提供真实的ID,Url
        /// </summary>
        public string ActiveUrl { set; get; }
        /// <summary>
        /// 活码真实的url
        /// </summary>
        public string RealUrl { set; get; }
        /// <summary>
        /// 活码展示内容,应用于真实的内部群
        /// </summary>
        public string FooerContent { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 当前内部码ID,扫码到频次时，切换码
        /// </summary>
        public long CurrentInnnerQrCodeId { set; get; }
        /// <summary>
        /// 客服活码ID，当QrType为客服时才应该有值
        /// </summary>
        public long? CustomServiceActivityId { set; get; }
        /// <summary>
        /// 内部二维码个数
        /// </summary>
        public int InnerQrCodeCount { set; get; }
        /// <summary>
        /// 报名用户ID
        /// </summary>
        public long? RegistrationUserId { set; get; }
        /// <summary>
        /// 报名用户姓名
        /// </summary>
        public string RegistrationUserName { set; get; }
        /// <summary>
        /// 报名费用
        /// </summary>
        public decimal? RegistrationFee { set; get; }
        /// <summary>
        /// 收入金额
        /// </summary>
        public decimal? RegistrationBonus { set; get; }
        /// <summary>
        /// 收入金额1级比列
        /// </summary>
        
        public decimal? RegistrationBonusLevelOneRatio { set; get; }
        /// <summary>
        /// 收入金额2级比列
        /// </summary>
        
        public decimal? RegistrationBonusLevelTwoRatio { set; get; }
        /// <summary>
        /// 追加金额比例
        /// </summary>
        
        public decimal? AdditionalAmountRatio { set; get; }
        /// <summary>
        /// 追加金额1级比例
        /// </summary>
        
        public decimal? AdditionalAmountLevelOneRatio { set; get; }
        /// <summary>
        /// 追加金额2级比例
        /// </summary>
        
        public decimal? AdditionalAmountLevelTwoRatio { set; get; }
        /// <summary>
        /// 报名后是否跳转到客服码
        /// </summary>
        public bool? JumpToCustomService { set; get; }
        /// <summary>
        /// 手机号码是否需要验证
        /// </summary>
        public bool? PhoneNumberNeedsVilidation { set; get; }
        /// <summary>
        /// 报名活码元素ID，多个使用","号隔开
        /// </summary>
        public string RegisterItemIds { set; get; }
        /// <summary>
        /// 报名活码实际应用样式:Code|Name|Type|HTML的格式用于快速处理页面的生成
        /// </summary>
        public string RegiterItemClass { set; get; }
    }
}