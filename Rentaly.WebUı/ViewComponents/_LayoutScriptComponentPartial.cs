using Microsoft.AspNetCore.Mvc;

namespace Rentaly.WebUI.ViewComponents
{
    public class _LayoutScriptComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
