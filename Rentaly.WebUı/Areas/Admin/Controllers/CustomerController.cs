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
            var allCustomers = await _customerService.TGetListAsync();
            var query = allCustomers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                         x.Surname.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                         x.IdentityNumber.Contains(search));
            }

            int totalFiltered = query.Count();
            int totalPages = (int)Math.Ceiling(totalFiltered / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages));

            var pagedData = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(_mapper.Map<List<ResultCustomerDTO>>(pagedData));
        }

        [HttpGet]
        public IActionResult CreateCustomer() => View();

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDTO createCustomerDTO)
        {
            var value = _mapper.Map<Customer>(createCustomerDTO);
            await _customerService.TInsertAsync(value);
            return RedirectToAction("CustomerList");
        }
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.TDeleteAsync(id);
            return RedirectToAction("CustomerList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var value = await _customerService.TGetByIdAsync(id);
            var mappedValue = _mapper.Map<UpdateCustomerDTO>(value);
            return View(mappedValue);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDTO updateCustomerDTO)
        {
            var value = _mapper.Map<Customer>(updateCustomerDTO);
            await _customerService.TUpdateAsync(value);
            return RedirectToAction("CustomerList");
        }
    }
}