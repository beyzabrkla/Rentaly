using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

[Area("Admin")]
public class AboutController : Controller
{
    private readonly IAboutService _aboutService;

    public AboutController(IAboutService aboutService)
    {
        _aboutService = aboutService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var values = await _aboutService.TGetListAsync();
        return View(values);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateAbout(int id)
    {
        var value = await _aboutService.TGetByIdAsync(id);

        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAbout(About about)
    {
        await _aboutService.TUpdateAsync(about);
        return RedirectToAction("Index");
    }
}