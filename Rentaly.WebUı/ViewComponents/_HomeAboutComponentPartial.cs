using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.ViewComponents
{
    public class _HomeAboutComponentPartial : ViewComponent
    {
        private readonly IAboutService _aboutService;

        public _HomeAboutComponentPartial(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _aboutService.TGetListAsync();
            var aboutData = values.FirstOrDefault();
            return View(aboutData);
        }
    }
}