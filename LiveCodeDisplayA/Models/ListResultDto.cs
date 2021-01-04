using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BJ.LiveCodeDisplay.Web.Models
{
    public class ListResultDto<T>
    {
        public IReadOnlyList<T> Items { get; set; }
    }
}