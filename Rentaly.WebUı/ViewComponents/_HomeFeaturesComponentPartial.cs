using Microsoft.AspNetCore.Mvc;

namespace Rentaly.WebUI.ViewComponents
{
    public class _HomeFeaturesComponentPartial :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(); 
        }
    }
}
