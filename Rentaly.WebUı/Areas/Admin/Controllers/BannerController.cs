using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

[Area("Admin")]
public class BannerController : Controller
{
    private readonly IBannerService _bannerService;

    public BannerController(IBannerService bannerService)
    {
        _bannerService = bannerService;
    }

    public async Task<IActionResult> Index()
    {
        var values = await _bannerService.TGetListAsync();
        return View(values);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateBanner(int id)
    {
        var value = await _bannerService.TGetByIdAsync(id);
        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBanner(Banner banner)
    {
        await _bannerService.TUpdateAsync(banner);
        return RedirectToAction("Index");
    }
}