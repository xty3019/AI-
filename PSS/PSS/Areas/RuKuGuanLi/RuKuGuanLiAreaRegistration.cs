using System.Web.Mvc;

namespace PSS.Areas.RuKuGuanLi
{
    public class RuKuGuanLiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RuKuGuanLi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RuKuGuanLi_default",
                "RuKuGuanLi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}