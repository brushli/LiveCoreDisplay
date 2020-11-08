using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiveCodeDisplayA.Models
{
    public class AbpResult<T>
    {
        public T result { set; get; }
    }
}