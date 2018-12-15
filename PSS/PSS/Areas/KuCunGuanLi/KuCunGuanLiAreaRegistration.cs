using System.Web.Mvc;

namespace PSS.Areas.KuCunGuanLi
{
    public class KuCunGuanLiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "KuCunGuanLi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "KuCunGuanLi_default",
                "KuCunGuanLi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}