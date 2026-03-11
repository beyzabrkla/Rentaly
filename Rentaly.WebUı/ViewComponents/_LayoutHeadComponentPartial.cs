using Microsoft.AspNetCore.Mvc;

namespace Rentaly.WebUI.ViewComponents
{
    public class _LayoutHeadComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
