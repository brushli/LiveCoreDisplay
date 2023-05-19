using BJ.LiveCodeDisplay.Web.Common;
using BJ.LiveCodeDisplay.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BJ.LiveCodeDisplay.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LoadWeChartPara();
        }
        private void LoadWeChartPara()
        {
            //获取信息
            var responseJson = HttpClientHelper.Get(WebConfig.GetWeChartParametersUrl, "");
            //转换为json对象
            AbpResult<GetWeChartParametersDto> chartSetResponse = JsonConvert.DeserializeObject<AbpResult<GetWeChartParametersDto>>(responseJson);
            if (chartSetResponse.success && chartSetResponse.result != null)
            {
                WebConfig.WeChatAppID = chartSetResponse.result.WeChatAppID;
                WebConfig.ICCCPOHomeIndexUrl = chartSetResponse.result.ICCCPOHomeIndexUrl;
                WebConfig.ScanCodeRedirectUrl = chartSetResponse.result.ScanCodeRedirectUrl;
                WebConfig.WeChatSecret = chartSetResponse.result.WeChatSecret;
                WebConfig.WeChatRedirectUrl = chartSetResponse.result.WeChatRedirectUrl;
            }
        }
    }
}
