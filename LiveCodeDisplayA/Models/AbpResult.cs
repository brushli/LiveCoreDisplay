using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BJ.LiveCodeDisplay.Web.Models
{
    public partial class AbpResult<T>
    {
        public T result { set; get; }
        public bool success { set; get; }
        public AbpError error { set; get; }
        public bool unAuthorizedRequest { set; get; }
        public bool __abp { set; get; }
    }
    public class AbpError
    {
        public string code { set; get; }
        public string message { set; get; }
        public string details { set; get; }
        public string validationErrors { set; get; }
       
    }
   
}