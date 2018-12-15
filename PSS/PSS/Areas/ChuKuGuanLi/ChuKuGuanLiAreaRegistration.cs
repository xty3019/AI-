using System.Web.Mvc;

namespace PSS.Areas.ChuKuGuanLi
{
    public class ChuKuGuanLiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ChuKuGuanLi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ChuKuGuanLi_default",
                "ChuKuGuanLi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}