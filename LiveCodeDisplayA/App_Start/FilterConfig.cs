using BJ.LiveCodeDisplay.Web.Attrubites;
using System.Web;
using System.Web.Mvc;

namespace BJ.LiveCodeDisplay.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionHandleErrorAttribute());
        }
    }
}
