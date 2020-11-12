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
        private readonly string GetWeChatUserInfoUrl = "http://106.15.72.245:5005/api/WeChat/GetWeChatUserInfo";
        private readonly string ScanCodeUrl = "http://106.15.72.245:5005/api/services/app/QrCodeActivitys/ScanCode";
        private readonly string LongPressUrl = "http://106.15.72.245:5005/api/services/app/QrCodeActivityInnerCode/LongPress";
        public ActionResult Index(string activityId, string userId, string ownerUserId, 
            string publicityId,string code,string state,string openid)
        {
            ViewBag.activityId = activityId;
            ViewBag.userId = userId;
            ViewBag.ownerUserId = ownerUserId;
            ViewBag.publicityId = publicityId;
            //return View();
            if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    //获取信息
                    //var requestJson = JsonConvert.SerializeObject(new { code, state });
                    //HttpContent httpContent = new StringContent(requestJson);
                    //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    //var httpClient = new HttpClient();
                    //var responseJson = httpClient.PostAsync(GetWeChatUserInfoUrl, httpContent).Result.Content.ReadAsStringAsync().Result;
                    ////转换为json对象
                    //AbpResult<GetWeChatUserInfoResult> userInfoResponse = JsonConvert.DeserializeObject<AbpResult<GetWeChatUserInfoResult>>(responseJson);
                    //if (userInfoResponse == null)
                    //{
                    //    return Content("未能获取您的信息，请重新扫码！");
                    //}
                    //requestJson = JsonConvert.SerializeObject(new
                    //{
                    //    userInfoResponse.result.openid,
                    //    userId,
                    //    activityId,
                    //    ownerUserId,
                    //    publicityId,
                    //});
                    //httpContent = new StringContent(requestJson);
                    //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    //responseJson = httpClient.PostAsync(ScanCodeUrl, httpContent).Result.Content.ReadAsStringAsync().Result;
                    //AbpResult<ScanQrCodeResult> scanCodeResult = JsonConvert.DeserializeObject<AbpResult<ScanQrCodeResult>>(responseJson);
                    //if (scanCodeResult == null)
                    //{
                    //    return Content("服务器出现错误，请稍后在扫码！" + responseJson);
                    //}
                    //else if (scanCodeResult.result.InnerCode != null && string.IsNullOrEmpty(scanCodeResult.result.InnerCode.HeaderImg))
                    //{
                    //    scanCodeResult.result.InnerCode.HeaderImg = "http://qrcodes-mskb.oss-cn-shanghai.aliyuncs.com/%E5%A4%B4%E5%83%8F.png";
                    //}
                    ViewBag.openid = openid ;
                    return View();
                }
                catch (Exception)
                {
                    return Content("请退出重新扫码！");
                }
            }
            else if (!string.IsNullOrEmpty(openid))
            {
                ViewBag.OpenID = "得到第二次:" + openid;
                //ViewBag.activityId = activityId;
                //ViewBag.userId = userId;
                //ViewBag.ownerUserId = ownerUserId;
                //ViewBag.publicityId = publicityId;
                //var requestJson = JsonConvert.SerializeObject(new
                //{
                //    openid,
                //    userId,
                //    activityId,
                //    ownerUserId,
                //    publicityId,
                //});
                //var httpClient = new HttpClient();
                //var httpContent = new StringContent(requestJson);
                //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //var responseJson = httpClient.PostAsync(ScanCodeUrl, httpContent).Result.Content.ReadAsStringAsync().Result;
                //AbpResult<ScanQrCodeResult> scanCodeResult = JsonConvert.DeserializeObject<AbpResult<ScanQrCodeResult>>(responseJson);
                //if (scanCodeResult == null)
                //{
                //    return Content("服务器出现错误，请稍后在扫码！" + responseJson);
                //}
                //else if (scanCodeResult.result.InnerCode != null && string.IsNullOrEmpty(scanCodeResult.result.InnerCode.HeaderImg))
                //{
                //    scanCodeResult.result.InnerCode.HeaderImg = "http://qrcodes-mskb.oss-cn-shanghai.aliyuncs.com/%E5%A4%B4%E5%83%8F.png";
                //}
                return View();
                //var redirect_uri = "http://ysy.hrtechsh.com/?" + "activityId=" + activityId + "&userId=" + userId + "&ownerUserId=" + ownerUserId + "&publicityId=" + publicityId;
                //redirect_uri = HttpUtility.UrlEncode(redirect_uri);
                ////引导页面，用ss来获取openid
                //var wecharUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxfb0c4f305db81aa6&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_base&state=scancode#wechat_redirect";
                //return Redirect(wecharUrl);
            }
            else if (string.IsNullOrEmpty(openid) && string.IsNullOrEmpty(code))
            {
                
                return View();
            }
            else
            {
                return Content("无效的访问！");
            }
        }
        public ActionResult RedirectToWeChat(string activityId, string userId, string ownerUserId, string publicityId)
        {
            //var redirect_uri = "http://ysy.hrtechsh.com/?" + "activityId=" + activityId + "&userId=" + userId + "&ownerUserId=" + ownerUserId + "&publicityId=" + publicityId;
            //redirect_uri = HttpUtility.UrlEncode(redirect_uri);
            ////引导页面，用ss来获取openid
            //var wecharUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxfb0c4f305db81aa6&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_base&state=scancode#wechat_redirect";
            //return Redirect(wecharUrl);
            return RedirectToAction("Index", new { openid = "111" });
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
            AbpResult<ResultDto> longPressResponse = JsonConvert.DeserializeObject<AbpResult<ResultDto>>(responseJson);
            return Json(longPressResponse);
        }
    }
}