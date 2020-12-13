using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BJ.LiveCodeDisplay.Web.Attrubites
{
    public class ExceptionHandleErrorAttribute: HandleErrorAttribute
    {
        /// <summary>
        /// 错误拦截
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            var result= new ContentResult();
            result.Content = $"<h1>服务器出现错误，请稍后在扫码！</h1>";
            filterContext.ExceptionHandled = true;
            filterContext.Result = result;
        }
    }
}