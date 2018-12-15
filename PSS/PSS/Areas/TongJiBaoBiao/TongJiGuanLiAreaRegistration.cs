using System.Web.Mvc;

namespace PSS.Areas.TongJiGuanLi
{
    public class TongJiGuanLiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TongJiGuanLi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TongJiGuanLi_default",
                "TongJiGuanLi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}