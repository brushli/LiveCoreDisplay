using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace BJ.LiveCodeDisplay.Web.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpClientHelper
    {
        /// <summary>
        /// 
        /// </summary>
        private static MediaTypeHeaderValue MediaTypeHeaderValue= new MediaTypeHeaderValue("application/json");
        /// <summary>
        /// 
        /// </summary>
        private static TimeSpan Timeout = new TimeSpan(0, 1, 0);//1分钟请求
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="inputJson"></param>
        /// <returns></returns>
        public static string Post(string url,string inputJson)
        {
            HttpContent httpContent = new StringContent(inputJson);
            httpContent.Headers.ContentType = MediaTypeHeaderValue;
            var httpClient = new HttpClient();
            httpClient.Timeout = Timeout;
            var response = httpClient.PostAsync(url, httpContent).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            return responseContent;
        }
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="inputJson"></param>
        /// <returns></returns>
        public static string Get(string url, string param)
        {
            var httpClient = new HttpClient();
            httpClient.Timeout = Timeout;
            var response = httpClient.GetAsync(url+"?"+ param).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            return responseContent;
        }
    }
}