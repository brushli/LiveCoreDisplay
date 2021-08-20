using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BJ.LiveCodeDisplay.Web.Models
{
    public class GetWeChartParametersDto
    {
        public string WeChartAppID { set; get; }
        public string WeChartSecret { set; get; }
        public string WeChartRedirectUrl { set; get; }
    }
}