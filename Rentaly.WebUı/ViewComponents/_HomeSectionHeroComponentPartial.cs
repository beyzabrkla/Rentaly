using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using System.Threading.Tasks;
using System.Linq;

namespace Rentaly.WebUI.ViewComponents
{
    public class _HomeSectionHeroComponentPartial : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IBranchService _branchService;
        private readonly IBannerService _bannerService;

        public _HomeSectionHeroComponentPartial(ICategoryService categoryService, IBranchService branchService, IBannerService bannerService)
        {
            _categoryService = categoryService;
            _branchService = branchService;
            _bannerService = bannerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.TGetListAsync();
            var branches = await _branchService.TGetListAsync();
            var banners = await _bannerService.TGetListAsync();

            ViewBag.Categories = categories;
            ViewBag.Branches = branches;

            var banner = banners.FirstOrDefault();
            return View(banner);
        }
    }
}