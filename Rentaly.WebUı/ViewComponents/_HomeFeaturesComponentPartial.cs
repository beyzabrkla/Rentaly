using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using System.Threading.Tasks;

namespace Rentaly.WebUI.ViewComponents
{
    public class _HomeFeaturesComponentPartial : ViewComponent
    {
        private readonly IProcessService _processService;

        public _HomeFeaturesComponentPartial(IProcessService processService)
        {
            _processService = processService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Process tablosundaki verileri çekiyoruz
            var values = await _processService.TGetListAsync();
            return View(values);
        }
    }
}