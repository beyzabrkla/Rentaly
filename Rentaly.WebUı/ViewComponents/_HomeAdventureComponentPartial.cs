using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using System.Threading.Tasks;
using System.Linq;

namespace Rentaly.WebUI.ViewComponents
{
    public class _HomeAdventureComponentPartial : ViewComponent
    {
        private readonly IProcessService _processService;

        public _HomeAdventureComponentPartial(IProcessService processService)
        {
            _processService = processService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _processService.TGetListAsync();
            return View(values.Take(3).ToList());
        }
    }
}