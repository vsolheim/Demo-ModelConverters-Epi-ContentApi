using System.Web.Mvc;
using EPiServer.Web.Mvc;

namespace DemoCustomModelConverters.Models.Pages.Start
{
    public class StartPageController : PageController<StartPage>
    {
        public ActionResult Index(StartPage currentPage)
        {
            return View("/Views/Pageviews/StartPage.cshtml", currentPage);
        }
    }
}