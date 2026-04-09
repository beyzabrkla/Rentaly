using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DTOLayer.CustomerDTOs;
using Rentaly.EntityLayer.Entities;
using AutoMapper;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> CustomerList(int page = 1, string? search = null)
        {
            int pageSize = 9;

            // 1. Sadece gerçek müşteri tablosundan verileri çek (Performans için en iyisi)
            var customers = await _customerService.TGetListAsync();

            var query = customers.AsQueryable();

            // 2. Arama Filtresi (Doğrudan query üzerinden)
            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                query = query.Where(x =>
                    (x.Name + " " + x.Surname).ToLower().Contains(s) ||
                    (x.IdentityNumber != null && x.IdentityNumber.Contains(s)) ||
                    (x.Email != null && x.Email.ToLower().Contains(s)));
            }

            // 3. Sayfalama Hesaplamaları
            int totalFiltered = query.Count();
            int totalPages = (int)Math.Ceiling(totalFiltered / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages));

            var pagedData = query
                .OrderByDescending(x => x.CreatedDate) // En yeni müşteriler en üstte
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mappedData = _mapper.Map<List<ResultCustomerDTO>>(pagedData);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchFilter = search;

            return View(mappedData);
        }

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.TDeleteAsync(id);
            return RedirectToAction("CustomerList");
        }
    }
}