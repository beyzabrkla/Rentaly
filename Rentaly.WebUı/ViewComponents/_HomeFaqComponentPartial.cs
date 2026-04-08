using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using System.Threading.Tasks;

namespace Rentaly.WebUI.ViewComponents
{
    public class _HomeFaqComponentPartial : ViewComponent
    {
        private readonly IFaqService _faqService;

        public _HomeFaqComponentPartial(IFaqService faqService)
        {
            _faqService = faqService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _faqService.TGetListAsync();
            return View(values);
        }
    }
}