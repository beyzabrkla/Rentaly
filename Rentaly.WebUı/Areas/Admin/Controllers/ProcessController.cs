using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

[Area("Admin")]
public class ProcessController : Controller
{
    private readonly IProcessService _processService;

    public ProcessController(IProcessService processService)
    {
        _processService = processService;
    }

    public async Task<IActionResult> Index()
    {
        var values = await _processService.TGetListAsync();
        return View(values);
    }

    [HttpGet]
    public IActionResult CreateProcess() => View();

    [HttpPost]
    public async Task<IActionResult> CreateProcess(Process process)
    {
        await _processService.TInsertAsync(process);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> UpdateProcess(int id)
    {
        var value = await _processService.TGetByIdAsync(id);
        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProcess(Process process)
    {
        await _processService.TUpdateAsync(process);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteProcess(int id)
    {
        await _processService.TDeleteAsync(id);
        return RedirectToAction("Index");
    }
}