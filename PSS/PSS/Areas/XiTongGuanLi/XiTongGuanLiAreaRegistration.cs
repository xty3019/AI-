using System.Web.Mvc;

namespace PSS.Areas.XiTongGuanLi
{
    public class XiTongGuanLiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "XiTongGuanLi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "XiTongGuanLi_default",
                "XiTongGuanLi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}