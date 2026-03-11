using Microsoft.AspNetCore.Mvc;

namespace Rentaly.WebUI.ViewComponents
{
    public class _LayoutHeaderComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
