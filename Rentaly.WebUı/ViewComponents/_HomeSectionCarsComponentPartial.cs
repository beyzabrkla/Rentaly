using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.ViewComponents
{
    public class _HomeSectionCarsComponentPartial:ViewComponent
    {
        private readonly ICarService _carService;

        public _HomeSectionCarsComponentPartial(ICarService carService)
        {
            _carService = carService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _carService.GetAvailableWithDetailsAsync();

            return View(values ?? new List<Car>());
        }
    }
}
