using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.DTOLayer.CarDTOs;

[Area("Admin")]
public class ReportController : Controller
{
    private readonly ICarService _carService;
    private readonly ICategoryService _categoryService;
    private readonly IBranchService _branchService;
    private readonly IRentalService _rentalService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ReportController(ICarService carService, ICategoryService categoryService, IBranchService branchService, IRentalService rentalService, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _carService = carService;
        _categoryService = categoryService;
        _branchService = branchService;
        _rentalService = rentalService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> ReportList(int? branchId, bool? status, int page = 1)
    {
        var cars = await _unitOfWork.CarDal.GetAllWithDetailsAsync();

        if (branchId.HasValue)
            cars = cars.Where(x => x.BranchId == branchId.Value).ToList();

        if (status.HasValue)
            cars = cars.Where(x => x.IsActive == status.Value).ToList();

        var dtoList = _mapper.Map<List<ResultCarDTO>>(cars);

        ViewBag.TotalCount = dtoList.Count;
        ViewBag.TotalValue = dtoList.Sum(x => x.DailyPrice);
        ViewBag.ActiveRate = dtoList.Count > 0 ? (dtoList.Count(x => x.IsActive) * 100 / dtoList.Count) : 0;

        ViewBag.Branches = await _unitOfWork.BranchDal.GetListAsync();

        ViewBag.CurrentBranch = branchId;
        ViewBag.CurrentStatus = status;

        int pageSize = 10;
        ViewBag.TotalPages = (int)Math.Ceiling((double)dtoList.Count / pageSize);
        ViewBag.CurrentPage = page;

        var pagedData = dtoList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return View(pagedData);
    }
    public async Task<IActionResult> ExportExcel(int? branchId, bool? status)
    {
        var cars = await _unitOfWork.CarDal.GetAllWithDetailsAsync();

        if (branchId.HasValue)
            cars = cars.Where(x => x.BranchId == branchId.Value).ToList();

        if (status.HasValue)
            cars = cars.Where(x => x.IsActive == status.Value).ToList();

        var dtoList = _mapper.Map<List<ResultCarDTO>>(cars);

        var branchName = branchId.HasValue
            ? (await _unitOfWork.BranchDal.GetByIdAsync(branchId.Value)).BranchName
            : "Tum-Subeler";

        var statusName = status.HasValue ? (status.Value ? "Aktif" : "Pasif") : "Tumu";
        string fileName = $"{branchName}-{statusName}-Filo-Listesi.xlsx";

        using (var workbook = new ClosedXML.Excel.XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Filo Raporu");
            worksheet.Cell(1, 1).Value = "Plaka";
            worksheet.Cell(1, 2).Value = "Marka";
            worksheet.Cell(1, 3).Value = "Model";
            worksheet.Cell(1, 4).Value = "Şube";
            worksheet.Cell(1, 5).Value = "Günlük Fiyat";

            for (int i = 0; i < dtoList.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = dtoList[i].PlateNumber;
                worksheet.Cell(i + 2, 2).Value = dtoList[i].BrandName;
                worksheet.Cell(i + 2, 3).Value = dtoList[i].CarModelName;
                worksheet.Cell(i + 2, 4).Value = dtoList[i].BranchName;
                worksheet.Cell(i + 2, 5).Value = dtoList[i].DailyPrice;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}