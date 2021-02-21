using BJ.LiveCodeDisplay.Web.Common;
using BJ.LiveCodeDisplay.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BJ.LiveCodeDisplay.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly static string BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
        private readonly string GetWeChatUserInfoUrl = BaseUrl + "api/WeChat/GetWeChatUserInfo";
        private readonly string ScanCodeUrl = BaseUrl + "api/services/app/QrCodeActivitys/ScanCode";
        private readonly string LongPressUrl = BaseUrl + "api/services/app/QrCodeActivityInnerCode/LongPress";
        private readonly string CommitSignUpUrl = BaseUrl + "api/services/app/QrCodeActivityRegister/Create";
        private readonly string SendMsgUrl = BaseUrl + "api/services/app/SmsMessageService/SendSignUpCode";
        private readonly string GetGradeUrl = BaseUrl + "api/services/app/Grade/GetAll";
        private readonly string OpenIdCookiesKey = "jzlm_openid";
        private readonly string WeChatRedirectUrl = ConfigurationManager.AppSettings["WeChatRedirectUrl"];
        private static List<GradeDto> GradeDtos = new List<GradeDto>();
        private readonly string WeChatAppID = ConfigurationManager.AppSettings["WeChatAppID"];
        /// <summary>
        /// 扫码
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <param name="ownerUserId"></param>
        /// <param name="publicityId"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult Index(string activityId, string userId, string ownerUserId, string publicityId, string code, string state)
        {
            if (string.IsNullOrEmpty(activityId) || string.IsNullOrWhiteSpace(activityId))
            {
                return Content("<h1>无效的扫码！</h1>");
            }
            //报名活码测试：https://localhost:44392/?activityId=29fdaf68-bbf9-4eb2-8d8f-42dc8b418da2&ownerUserId=2&userId=2
            ViewBag.activityId = activityId;
            ViewBag.userId = userId;
            ViewBag.ownerUserId = ownerUserId;
            ViewBag.publicityId = publicityId;
            //var openId = Request.Cookies[OpenIdCookiesKey];
            var openId =new HttpCookie("aa","cccc");
            if (openId != null)
            {
                ViewBag.openid = openId.Value;
                return ScanCodeGetView(activityId, userId, ownerUserId, publicityId, openId.Value);
            }
            else if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    var weChatUserInfo = GetWeChatUserInfo(code, state);
                    if (weChatUserInfo == null)
                    {
                        return Content("<h1>未能获取您的信息，请重新扫码</h1>！");
                    }
                    Response.Cookies.Add(new HttpCookie(OpenIdCookiesKey)
                    {
                        Value = weChatUserInfo.openid,
                        Expires = DateTime.Now.AddDays(2000)
                    });
                    ViewBag.openid = weChatUserInfo.openid;
                    return ScanCodeGetView(activityId, userId, ownerUserId, publicityId, weChatUserInfo.openid);
                }
                catch (Exception)
                {
                    var redirect_uri = WeChatRedirectUrl + "?activityId=" + activityId + "&userId=" + userId + "&ownerUserId=" + ownerUserId + "&publicityId=" + publicityId;
                    redirect_uri = HttpUtility.UrlEncode(redirect_uri);
                    //引导页面，用ss来获取openid
                    var wecharUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + WeChatAppID + "&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_base&state=scancode#wechat_redirect";
                    return Redirect(wecharUrl);
                }
            }
            else
            {
                var redirect_uri = WeChatRedirectUrl + "?activityId=" + activityId + "&userId=" + userId + "&ownerUserId=" + ownerUserId + "&publicityId=" + publicityId;
                redirect_uri = HttpUtility.UrlEncode(redirect_uri);
                //引导页面，用ss来获取openid
                var wecharUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + WeChatAppID + "&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_base&state=scancode#wechat_redirect";
                return Redirect(wecharUrl);
            }
        }
        /// <summary>
        /// 扫码并返回需要查看的视图
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <param name="ownerUserId"></param>
        /// <param name="publicityId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        private ActionResult ScanCodeGetView(string activityId, string userId, string ownerUserId, string publicityId, string openId)
        {
            var scanCodeResult = ScanCode(openId, activityId, userId, ownerUserId, publicityId);
            //报名的活码
            if (scanCodeResult.IsSucess && scanCodeResult.QrCodeActivity != null && scanCodeResult.QrCodeActivity.QrType == "报名")
            {
                return SignUp(scanCodeResult);
            }
            else if (scanCodeResult.IsSucess && scanCodeResult.InnerCode != null)
            {
                ViewBag.innerCodeId = scanCodeResult.InnerCode.Id;
                return View("Index", scanCodeResult);
            }
            else
            {
                return Content($"<h1>{scanCodeResult.Message}</h1>");
            }
        }

        /// <summary>
        /// 长按二维码
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <param name="ownerUserId"></param>
        /// <param name="publicityId"></param>
        /// <param name="openId"></param>
        /// <param name="innerCodeId"></param>
        /// <returns></returns>
        public ActionResult LongPress(string activityId, string userId, string ownerUserId, string publicityId, string openId, string innerCodeId)
        {
            //获取信息
            var requestJson = JsonConvert.SerializeObject(new { activityId, userId, ownerUserId, publicityId, openId, innerCodeId });
            HttpContent httpContent = new StringContent(requestJson);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var httpClient = new HttpClient();
            var responseJson = httpClient.PostAsync(LongPressUrl, httpContent).Result.Content.ReadAsStringAsync().Result;
            //转换为json对象
            //转换为json对象
            AbpResult<ResultDto> longPressResponse = JsonConvert.DeserializeObject<AbpResult<ResultDto>>(responseJson);
            return Json(longPressResponse);
        }
        public ActionResult TestIndex()
        {
            ViewBag.ss = "http://qrcodes-mskb.oss-cn-shanghai.aliyuncs.com/%E7%BE%A4%E7%A0%81/%E6%95%88%E6%9E%9C%E5%9B%BE/eb20243d-65ad-4e70-9cf2-04de8bdd179b.jpg";
            return View();
        }
        /// <summary>
        /// 报名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult SignUp(ScanQrCodeResult scanQrCodeResult)
        {
            var input = scanQrCodeResult.QrCodeActivity;
            ViewBag.activityId = input.Id;
            if (!string.IsNullOrEmpty(input.RegiterItemClass) && !string.IsNullOrWhiteSpace(input.RegiterItemClass))
            {
                string html = $"<form role=\"form\" id='signUpForm' action=\"{Url.Action("SignUpCommit")}\" method=\"post\" autocomplete=\"off\">";
                html += $"<fieldset><input type=\"hidden\"  id=\"ActivityId\" name=\"ActivityId\" value=\"{input.Id}\" />";
                html += $"<input type=\"hidden\"  id=\"userId\" name=\"userId\" value=\"{ViewBag.userId}\" />";
                html += $"<input type=\"hidden\"  id=\"ownerUserId\" name=\"ownerUserId\" value=\"{ViewBag.ownerUserId}\" />";
                html += $"<input type=\"hidden\"  id=\"publicityId\" name=\"publicityId\" value=\"{ViewBag.publicityId}\" />";
                html += $"<input type=\"hidden\"  id=\"JumpToCustomService\" name=\"JumpToCustomService\" value=\"{input.JumpToCustomService}\" />";
                html += $"<input type=\"hidden\"  id=\"CustomServiceUrl\" name=\"CustomServiceUrl\" value=\"{scanQrCodeResult.CustomServiceUrl}\" />";
                html += $"<input type=\"hidden\"  id=\"openid\" name=\"openid\" value=\"{ViewBag.openid}\" />";
                html += $"<input type=\"hidden\"  id=\"RegistrationUserId\" name=\"RegistrationUserId\" value=\"{input.RegistrationUserId}\" />";
                html += $"<input type=\"hidden\"  id=\"RegistrationUserName\" name=\"RegistrationUserName\" value=\"{input.RegistrationUserName}\" />";
                //html += $"<div id=\"legend\" class=\"text-center\"><legend class=\"\">{input.Name}</legend></div>";
                if (!string.IsNullOrWhiteSpace(input.RegisterImageUrl))
                {
                    html += $"<img id=\"topImg\" class=\"img-responsive\" src=\"{input.RegisterImageUrl}\"></img>";
                }
                var itemClass = input.RegiterItemClass.Split(',');
                foreach (var item in itemClass)
                {
                    var items = item.Split('|');
                    if (items.Length == 4)
                    {
                        html += $"<div class=\"form-group\"><label for=\"{items[0]}\" class=\"col-sm-2 control-label\">{items[1]}</label>";
                        string htmlType;
                        string htmlClass;
                        switch (items[2])
                        {
                            case "文本框":
                            case "身份证号码":
                            case "邮箱":
                                htmlClass = "form-control";
                                htmlType = "text";
                                html += $"<div class=\"col-sm-10\"><input type=\"{htmlType}\" class=\"{htmlClass}\" id=\"{items[0]}\" name=\"{items[0]}\"></div>";
                                break;
                            case "地址":
                                html += "<div ><div id=\"distpicker\" ><div class=\"form-group\"><div style=\"position: relative; \"><input id=\"city-picker3\" class=\"form-control\" readonly type=\"text\"  name='RangeAddress' Id='RangeAddress' data-toggle=\"city-picker\"></div></div></div></div>";
                                break;
                            case "日期":
                                htmlClass = "form-control datepicker";
                                htmlType = "text";
                                html += $"<div class=\"col-sm-10\"><input type=\"{htmlType}\" class=\"{htmlClass}\" id=\"{items[0]}\" name=\"{items[0]}\"></div>";
                                break;
                            case "单选框":
                                htmlType = "radio";
                                htmlClass = "form-control";
                                html += $"<div class=\"col-sm-10\"><input type=\"{htmlType}\" class=\"{htmlClass}\" id=\"{items[0]}\" name=\"{items[0]}\"></div>";
                                break;
                            case "下拉选项":
                                html += $"<div class=\"col-sm-10\">  <select class=\"form-control\" id=\"{items[0]}\" name=\"{items[0]}\">";
                                if (items[1] == "选择班型" && !string.IsNullOrEmpty(input.ClassType))
                                {
                                    var classType = input.ClassType.Split(',');

                                    foreach (var classTy in classType)
                                    {
                                        html += $"<option value=\"{classTy}\">{classTy}</option>";
                                    }
                                }
                                else if (items[1] == "年级")
                                {
                                    GetGradeAll();
                                    foreach (var grade in GradeDtos)
                                    {
                                        html += $"<option value=\"{grade.Name}\">{grade.Name}</option>";
                                    }
                                }
                                html += "</select></div>";
                                break;
                            case "手机号码":
                                htmlType = "text";
                                htmlClass = "form-control";
                                html += $"<div class=\"col-sm-10\"><input type=\"{htmlType}\" class=\"{htmlClass}\" id=\"{items[0]}\" name=\"{items[0]}\"></div>";
                                html += "</div>";
                                if (input.PhoneNumberNeedsVilidation.HasValue && input.PhoneNumberNeedsVilidation.Value)
                                {
                                    html += " <div class=\"input-group\"><div class=\"col-sm-10 top-left\"><input class=\"btn btn-info\" type=\"button\" id=\"getcode\" value=\"点击获取手机验证码\" /><span id = \"telephonenameTip\" ></span></div></div>";
                                    htmlType = "text";
                                    htmlClass = "form-control";
                                    html += $"<div class=\"form-group\"><label for=\"SmsCode\" class=\"col-sm-2 control-label\" style=\"margin-top:10px\">验证码</label>";
                                    html += $"<div class=\"col-sm-10\"><input type=\"{htmlType}\" class=\"{htmlClass}\" id=\"SmsCode\" name=\"SmsCode\"></div>";
                                }
                                break;
                            default:
                                htmlType = "text";
                                htmlClass = "form-control";
                                break;
                        }
                        html += "</div>";
                    }
                }
                html += "<div class=\"form-group text-center\"><button type=\"button\" id=\"submitBtn\" name=\"submit\" class=\"btn btn-primary btn-lg\"> <span class=\"glyphicon glyphicon-floppy-disk\" aria-hidden=\"true\"></span>报名</button><div></fieldset></form>";
                ViewBag.ContentHtml = html;
            }
            return View("SignUp", input);
        }
        /// <summary>
        /// 报名提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SignUpCommit(QrCodeActivityRegister input)
        {
            try
            {
                //获取信息
                var requestJson = JsonConvert.SerializeObject(input);
                var responseJson = HttpClientHelper.Post(CommitSignUpUrl, requestJson);
                //转换为json对象
                AbpResult<QrCodeActivityRegister> abpResult = JsonConvert.DeserializeObject<AbpResult<QrCodeActivityRegister>>(responseJson);
                if (abpResult != null && abpResult.result != null && abpResult.result.Id > 0)
                {
                    /* if (input.JumpToCustomService.HasValue && input.JumpToCustomService.Value && !string.IsNullOrWhiteSpace(input.CustomServiceUrl))
                     {
                         return RedirectPermanent(input.CustomServiceUrl);
                     }*/
                    return Json(new
                    {
                        success = true,
                        input.JumpToCustomService,
                        input.CustomServiceUrl
                    }, JsonRequestBehavior.AllowGet);
                }
                else if (!abpResult.success)
                {
                    return Json(new { success = false, msg = abpResult.error?.message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, msg = "报名失败，请稍后再试！" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 发送报名注册手机验证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendSignUpSms(string mobile)
        {
            try
            {
                //获取信息
                var requestJson = JsonConvert.SerializeObject(new { mobile });
                var responseJson = HttpClientHelper.Post(SendMsgUrl, requestJson);
                //转换为json对象
                AbpResult<SendSmsResultDto> abpResult = JsonConvert.DeserializeObject<AbpResult<SendSmsResultDto>>(responseJson);
                if (abpResult != null && abpResult.result != null && abpResult.result.Success)
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, msg = "请稍后再试！" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <param name="ownerUserId"></param>
        /// <param name="publicityId"></param>
        /// <returns></returns>
        private ScanQrCodeResult ScanCode(string openId, string activityId, string userId, string ownerUserId, string publicityId)
        {
            ScanQrCodeResult result = new ScanQrCodeResult();
            try
            {
                var httpClient = new HttpClient();
                var requestJson = JsonConvert.SerializeObject(new
                {
                    openid = openId,
                    userId,
                    activityId,
                    ownerUserId,
                    publicityId,
                });
                var responseJson = HttpClientHelper.Post(ScanCodeUrl, requestJson);
                AbpResult<ScanQrCodeResult> scanCodeResult = JsonConvert.DeserializeObject<AbpResult<ScanQrCodeResult>>(responseJson);
                if (scanCodeResult != null && scanCodeResult.result != null)
                {
                    result = scanCodeResult.result;
                }
                else if (scanCodeResult == null || scanCodeResult.result == null)
                {
                    result.IsSucess = false;
                    result.Message = $"<h1>服务器出现错误，请稍后在扫码！{responseJson}</h1>";

                }
                else if (scanCodeResult.result != null && scanCodeResult.result.InnerCode == null && scanCodeResult.result.QrCodeActivity == null)
                {
                    result.IsSucess = false;
                    result.Message = $"<h1>{scanCodeResult.result.Message}</h1>";
                }
                else if (scanCodeResult.result.InnerCode != null && string.IsNullOrEmpty(scanCodeResult.result.InnerCode.HeaderImg))
                {
                    scanCodeResult.result.InnerCode.HeaderImg = "http://qrcodes-mskb.oss-cn-shanghai.aliyuncs.com/%E5%A4%B4%E5%83%8F.png";
                }
            }
            catch (Exception ex)
            {
                result.IsSucess = false;
                result.Message = $"扫码错误：{ex.Message}</h1>";
            }
            return result;
        }
        /// <summary>
        /// 获取微信用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private GetWeChatUserInfoResult GetWeChatUserInfo(string code, string state)
        {
            try
            {
                //获取信息
                var requestJson = JsonConvert.SerializeObject(new { code, state });
                var responseJson = HttpClientHelper.Post(GetWeChatUserInfoUrl, requestJson);
                //转换为json对象
                AbpResult<GetWeChatUserInfoResult> userInfoResponse = JsonConvert.DeserializeObject<AbpResult<GetWeChatUserInfoResult>>(responseJson);
                if (userInfoResponse == null || userInfoResponse.result == null)
                {
                    return null;
                }
                else
                {
                    return userInfoResponse.result;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }
        /// <summary>
        /// 获取年级
        /// </summary>
        private void GetGradeAll()
        {
            try
            {
                if (GradeDtos.Count == 0)
                {
                    //获取信息
                    //var requestJson = JsonConvert.SerializeObject(new { SkipCount=0, MaxResultCount=int.MaxValue });
                    var responseJson = HttpClientHelper.Get(GetGradeUrl, $"SkipCount=0&MaxResultCount={int.MaxValue}");
                    //转换为json对象
                    AbpResult<PagedResultDto<GradeDto>> gradeResponse = JsonConvert.DeserializeObject<AbpResult<PagedResultDto<GradeDto>>>(responseJson);
                    GradeDtos = gradeResponse.result.Items.ToList();
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}