using System.Web.Mvc;

namespace PSS.Areas.CaiWuGuanLi
{
    public class CaiWuGuanLiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CaiWuGuanLi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CaiWuGuanLi_default",
                "CaiWuGuanLi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}