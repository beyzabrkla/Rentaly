using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using System.Threading.Tasks;

namespace Rentaly.WebUI.ViewComponents
{
    public class _HomeTestimonialComponentPartial : ViewComponent
    {
        private readonly ITestimonialService _testimonialService;

        public _HomeTestimonialComponentPartial(ITestimonialService testimonialService)
        {
            _testimonialService = testimonialService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _testimonialService.TGetListAsync();
            return View(values);
        }
    }
}