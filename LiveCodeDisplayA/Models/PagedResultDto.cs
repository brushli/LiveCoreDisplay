using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BJ.LiveCodeDisplay.Web.Models
{
    public class PagedResultDto<T> : ListResultDto<T>
    {
        public int TotalCount { get; set; }
    }
}