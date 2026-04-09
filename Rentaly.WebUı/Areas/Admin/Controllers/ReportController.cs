using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.DTOLayer.CarDTOs;

[Area("Admin")]
public class ReportController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ReportController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    private async Task<List<ResultCarDTO>> GetFilteredCarsAsync(int? branchId, bool? status, string rent, string search)
    {
        var cars = await _unitOfWork.CarDal.GetAllWithDetailsAsync();

        if (branchId.HasValue) cars = cars.Where(x => x.BranchId == branchId.Value).ToList();
        if (status.HasValue) cars = cars.Where(x => x.IsActive == status.Value).ToList();

        if (rent == "available") cars = cars.Where(x => x.IsAvailable).ToList();
        else if (rent == "rented") cars = cars.Where(x => !x.IsAvailable).ToList();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            cars = cars.Where(x =>
                (x.PlateNumber != null && x.PlateNumber.ToLower().Contains(search)) ||
                (x.Brand != null && x.Brand.BrandName.ToLower().Contains(search)) ||
                (x.CarModel != null && x.CarModel.ModelName.ToLower().Contains(search))
            ).ToList();
        }

        return _mapper.Map<List<ResultCarDTO>>(cars);
    }

    public async Task<IActionResult> ReportList(int? branchId, bool? status, string rent, string sort, string search, int page = 1)
    {
        var dtoList = await GetFilteredCarsAsync(branchId, status, rent, search);

        // Sıralama
        dtoList = sort switch
        {
            "price_asc" => dtoList.OrderBy(x => x.DailyPrice).ToList(),
            "price_desc" => dtoList.OrderByDescending(x => x.DailyPrice).ToList(),
            _ => dtoList
        };

        ViewBag.TotalCount = dtoList.Count;
        ViewBag.ActiveCount = dtoList.Count(x => x.IsActive);

        //Aktiflik Oranı Hesaplama
        ViewBag.ActiveRate = dtoList.Count > 0
            ? (int)Math.Round((double)dtoList.Count(x => x.IsActive) / dtoList.Count * 100)
            : 0;

        ViewBag.TotalValue = dtoList.Sum(x => x.DailyPrice);
        ViewBag.AvgPrice = dtoList.Count > 0 ? (int)dtoList.Average(x => x.DailyPrice) : 0;
        ViewBag.Branches = await _unitOfWork.BranchDal.GetListAsync();

        const int pageSize = 10;
        int totalPages = (int)Math.Ceiling((double)dtoList.Count / pageSize);
        ViewBag.TotalPages = totalPages;
        ViewBag.CurrentPage = page;

        var pagedData = dtoList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return View(pagedData);
    }
    public async Task<IActionResult> ExportExcel(int? branchId, bool? status, string rent, string search)
    {
        var dtoList = await GetFilteredCarsAsync(branchId, status, rent, search);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Filo Raporu");

        string[] headers = { "Plaka", "Marka", "Model", "Şube", "Fiyat", "KM", "Kira Durumu", "Durum" };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#a855f7");
            cell.Style.Font.FontColor = XLColor.White;
        }

        for (int i = 0; i < dtoList.Count; i++)
        {
            var item = dtoList[i];
            int row = i + 2;
            worksheet.Cell(row, 1).Value = item.PlateNumber;
            worksheet.Cell(row, 2).Value = item.BrandName;
            worksheet.Cell(row, 3).Value = item.CarModelName;
            worksheet.Cell(row, 4).Value = item.BranchName;
            worksheet.Cell(row, 5).Value = (double)item.DailyPrice;
            worksheet.Cell(row, 6).Value = item.Kilometer;
            worksheet.Cell(row, 7).Value = item.IsAvailable ? "Müsait" : "Kirada";
            worksheet.Cell(row, 8).Value = item.IsActive ? "Aktif" : "Pasif";
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Filo-Raporu.xlsx");
    }

    public async Task<IActionResult> ExportPdf(int? branchId, bool? status, string rent, string search)
    {
        var dtoList = await GetFilteredCarsAsync(branchId, status, rent, search);

        ViewBag.ReportDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        ViewBag.TotalCount = dtoList.Count;

        return View(dtoList);
    }
}