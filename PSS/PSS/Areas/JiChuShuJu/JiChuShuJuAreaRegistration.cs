using System.Web.Mvc;

namespace PSS.Areas.JiChuShuJu
{
    public class JiChuShuJuAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "JiChuShuJu";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "JiChuShuJu_default",
                "JiChuShuJu/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}