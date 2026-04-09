using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

[Area("Admin")]
public class FAQController : Controller
{
    private readonly IFaqService _faqService;

    public FAQController(IFaqService faqService)
    {
        _faqService = faqService;
    }

    public async Task<IActionResult> Index()
    {
        var values = await _faqService.TGetListAsync();
        return View(values);
    }

    [HttpGet]
    public IActionResult CreateFAQ() => View();

    [HttpPost]
    public async Task<IActionResult> CreateFAQ(FAQ faq)
    {
        await _faqService.TInsertAsync(faq);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> UpdateFAQ(int id)
    {
        var value = await _faqService.TGetByIdAsync(id);
        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateFAQ(FAQ faq)
    {
        await _faqService.TUpdateAsync(faq);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteFAQ(int id)
    {
        await _faqService.TDeleteAsync(id);
        return RedirectToAction("Index");
    }
}