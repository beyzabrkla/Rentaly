using Microsoft.AspNetCore.Mvc;

namespace Rentaly.WebUI.ViewComponents
{
    public class _HomeFaqComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
