using LiveCodeDisplayA.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LiveCodeDisplayA.Controllers
{
    public class HomeController : Controller
    {
        private readonly string GetWeChatUserInfoUrl = "http://localhost:5000/api/WeChat/GetWeChatUserInfo";
        private readonly string ScanCodeUrl = "http://localhost:5000/api/services/app/QrCodeActivitys/ScanCode";
        private readonly string LongPressUrl = "http://localhost:5000/api/services/app/QrCodeActivityInnerCode/LongPress";
        private readonly string OpenIdCookiesKey = "jzlm_openid";
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
            if (string.IsNullOrEmpty(activityId)||string.IsNullOrWhiteSpace(activityId))
            {
                return Content("<h1>无效的扫码！</h1>");
            }
            //报名活码测试：https://localhost:44392/?activityId=29fdaf68-bbf9-4eb2-8d8f-42dc8b418da2&ownerUserId=2&userId=2
            ViewBag.activityId = activityId;
            ViewBag.userId = userId;
            ViewBag.ownerUserId = ownerUserId;
            ViewBag.publicityId = publicityId;
            var openId = Request.Cookies[OpenIdCookiesKey];
            openId = new HttpCookie("OpenIdCookiesKey", "bbbb");
            if (openId != null)
            {
                ViewBag.openid = openId.Value;
                var scanCodeResult = ScanCode(openId.Value, activityId, userId, ownerUserId, publicityId);
                //报名的活码
                if (scanCodeResult.QrCodeActivity!=null&& scanCodeResult.QrCodeActivity.QrType== "报名")
                {
                    return SignUp(scanCodeResult.QrCodeActivity);
                }
                if (scanCodeResult.IsSucess && scanCodeResult.InnerCode != null)
                {
                    ViewBag.innerCodeId = scanCodeResult.InnerCode.Id;
                }
                else
                {
                    return Content(scanCodeResult.Message);
                }
                return View(scanCodeResult);
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
                    var scanCodeResult = ScanCode(weChatUserInfo.openid, activityId, userId, ownerUserId, publicityId);
                    //报名的活码
                    if (scanCodeResult.QrCodeActivity != null && scanCodeResult.QrCodeActivity.QrType == "报名")
                    {
                        return SignUp(scanCodeResult.QrCodeActivity);
                    }
                    if (scanCodeResult.IsSucess && scanCodeResult.InnerCode != null)
                    {
                        ViewBag.innerCodeId = scanCodeResult.InnerCode.Id;
                    }
                    else
                    {
                        return Content(scanCodeResult.Message);
                    }
                    return View(scanCodeResult);
                }
                catch (Exception)
                {
                    var redirect_uri = "http://ysy.hrtechsh.com/?" + "activityId=" + activityId + "&userId=" + userId + "&ownerUserId=" + ownerUserId + "&publicityId=" + publicityId;
                    redirect_uri = HttpUtility.UrlEncode(redirect_uri);
                    //引导页面，用ss来获取openid
                    var wecharUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxfb0c4f305db81aa6&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_base&state=scancode#wechat_redirect";
                    return Redirect(wecharUrl);
                }
            }
            else
            {
                var redirect_uri = "http://ysy.hrtechsh.com/?" + "activityId=" + activityId + "&userId=" + userId + "&ownerUserId=" + ownerUserId + "&publicityId=" + publicityId;
                redirect_uri = HttpUtility.UrlEncode(redirect_uri);
                //引导页面，用ss来获取openid
                var wecharUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxfb0c4f305db81aa6&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_base&state=scancode#wechat_redirect";
                return Redirect(wecharUrl);
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
        public ActionResult SignUp(QrCodeActivitys input)
        {
            //ViewBag.ss = "http://qrcodes-mskb.oss-cn-shanghai.aliyuncs.com/%E7%BE%A4%E7%A0%81/%E6%95%88%E6%9E%9C%E5%9B%BE/eb20243d-65ad-4e70-9cf2-04de8bdd179b.jpg";
            if (!string.IsNullOrEmpty(input.RegiterItemClass)&&!string.IsNullOrWhiteSpace(input.RegiterItemClass))
            {
                string html = "<form role=\"form\" action=\"Home\\SignUpCommit\" method=\"post\" autocomplete=\"off\">";
                html += "<fieldset><div id=\"legend\" class=\"text-center\"><legend class=\"\">报名信息填写</legend></div>";
                var itemClass = input.RegiterItemClass.Split(',');
                foreach (var item in itemClass)
                {
                    var items = item.Split('|');
                    if (items.Length == 4)
                    {
                        switch (items[2])
                        {
                            case "文本框":
                                html += $"<div class=\"form-group\"><label for=\"{items[0]}\" class=\"col-sm-2 control-label\">{items[1]}</label>";
                                html += $"<div class=\"col-sm-10\"><input type=\"text\" class=\"form-control\" id=\"{items[0]}\" placeholder=\"请输入{items[1]}\"></div></div>";
                                break;
                            case "日期":
                                html += $"<div class=\"form-group\"><label for=\"{items[0]}\" class=\"col-sm-2 control-label\">{items[1]}</label>";
                                html += $"<div class=\"input-group from-date\"> <input type=\"text\" class=\"form-control\"/><span class=\"input-group-addon\"><span class=\"glyphicon glyphicon-calendar\"></span></span></div></div>";
                                break;
                            case "单选框":
                                html += $"<div class=\"form-group\"><label for=\"{items[0]}\" class=\"col-sm-2 control-label\">{items[1]}</label>";
                                html += $"<div class=\"col-sm-10\"><input type=\"radio\" class=\"form-control\" id=\"{items[0]}\" ></div></div>";
                                break;
                            case "手机号码":
                                html += $"<div class=\"form-group\"><label for=\"{items[0]}\" class=\"col-sm-2 control-label\">{items[1]}</label>";
                                html += $"<div class=\"col-sm-10\"><input type=\"text\" class=\"form-control\" id=\"{items[0]}\" placeholder=\"请输入{items[1]}\"></div></div>";
                                break;
                            case "下拉选项":
                                html += $"<div class=\"form-group\"><label for=\"{items[0]}\" class=\"col-sm-2 control-label\">{items[1]}</label>";
                                html += $"<div class=\"col-sm-10\"><input type=\"text\" class=\"form-control\" id=\"{items[0]}\" placeholder=\"请输入{items[1]}\"></div></div>";
                                break;
                            case "地址":
                                html += $"<div class=\"form-group\"><label for=\"{items[0]}\" class=\"col-sm-2 control-label\">{items[1]}</label>";
                                html += $"<div class=\"col-sm-10\"><input type=\"text\" class=\"form-control\" id=\"{items[0]}\" placeholder=\"请输入{items[1]}\"></div></div>";
                                break;
                            case "邮箱":
                                html += $"<div class=\"form-group\"><label for=\"{items[0]}\" class=\"col-sm-2 control-label\">{items[1]}</label>";
                                html += $"<div class=\"col-sm-10\"><input type=\"text\" class=\"form-control\" id=\"{items[0]}\" placeholder=\"请输入{items[1]}\"></div></div>";
                                break;
                            case "身份证号码":
                                html += $"<div class=\"form-group\"><label for=\"{items[0]}\" class=\"col-sm-2 control-label\">{items[1]}</label>";
                                html += $"<div class=\"col-sm-10\"><input type=\"text\" class=\"form-control\" id=\"{items[0]}\" placeholder=\"请输入{items[1]}\"></div></div>";
                                break;
                            default:
                                break;
                        }
                    }
                }
                html += "<div class=\"form-group text-center\"><button type=\"button\" id=\"submit\" name=\"submit\" class=\"btn btn-primary\">报名</button><div></fieldset></form>";
                ViewBag.ContentHtml = html;
            }
            return View("SignUp");
        }
        /// <summary>
        /// 报名提交
        /// </summary>
        /// <returns></returns>
        public JsonResult SignUpCommit() 
        {
            return Json(new { success = false });
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
        private ScanQrCodeResult ScanCode(string openId,string activityId, string userId, string ownerUserId, string publicityId)
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
                var httpContent = new StringContent(requestJson);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var responseJson = httpClient.PostAsync(ScanCodeUrl, httpContent).Result.Content.ReadAsStringAsync().Result;
                AbpResult<ScanQrCodeResult> scanCodeResult = JsonConvert.DeserializeObject<AbpResult<ScanQrCodeResult>>(responseJson);
                if (scanCodeResult == null)
                {
                    result.IsSucess = false;
                    result.Message = $"<h1>服务器出现错误，请稍后在扫码！{responseJson}</h1>";

                }
                else if (scanCodeResult.result != null && scanCodeResult.result.InnerCode == null)
                {
                    result.IsSucess = false;
                    result.Message = $"<h1>{scanCodeResult.result.Message}</h1>";
                }
                else if (scanCodeResult.result.InnerCode != null && string.IsNullOrEmpty(scanCodeResult.result.InnerCode.HeaderImg))
                {
                    scanCodeResult.result.InnerCode.HeaderImg = "http://qrcodes-mskb.oss-cn-shanghai.aliyuncs.com/%E5%A4%B4%E5%83%8F.png";
                }
                result = scanCodeResult.result;
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
                HttpContent httpContent = new StringContent(requestJson);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var httpClient = new HttpClient();
                var responseJson = httpClient.PostAsync(GetWeChatUserInfoUrl, httpContent).Result.Content.ReadAsStringAsync().Result;
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
    }
}