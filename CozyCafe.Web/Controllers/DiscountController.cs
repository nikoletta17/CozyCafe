using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.Domain;
using CozyCafe.Models.DTO.Admin;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CozyCafe.Web.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;

        public DiscountController(IDiscountService discountService, IMapper mapper)
        {
            _discountService = discountService;
            _mapper = mapper;
        }

        // GET: Discount - список усіх знижок
        public async Task<IActionResult> Index()
        {
            var discounts = await _discountService.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<DiscountDto>>(discounts);
            return View(dtoList);
        }

        // GET: Details/id 
        public async Task<IActionResult> Details(int id)
        {
            var discount = await _discountService.GetByIdAsync(id);
            if (discount == null)
                return NotFound();

            var dto = _mapper.Map<DiscountDto>(discount);
            return View(dto);
        }

        // GET: Create - форма створення нової знижки
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create - обробка форми створення знижки
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscountDto discountDto)
        {
            if (!ModelState.IsValid)
                return View(discountDto);

            var discount = _mapper.Map<Discount>(discountDto);
            discount.ValidFrom = System.DateTime.UtcNow; 

            await _discountService.AddDiscountAsync(discount);
            return RedirectToAction(nameof(Index));
        }

        // GET: Edit/id - форма редагування знижки
        public async Task<IActionResult> Edit(int id)
        {
            var discount = await _discountService.GetByIdAsync(id);
            if (discount == null)
                return NotFound();

            var dto = _mapper.Map<DiscountDto>(discount);
            return View(dto);
        }

        // POST: Edit/id - обробка форми редагування
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DiscountDto discountDto)
        {
            if (id != discountDto.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(discountDto);

            var discount = _mapper.Map<Discount>(discountDto);
            await _discountService.UpdateDiscountAsync(discount);

            return RedirectToAction(nameof(Index));
        }

        // GET: Delete/id - підтвердження видалення
        public async Task<IActionResult> Delete(int id)
        {
            var discount = await _discountService.GetByIdAsync(id);
            if (discount == null)
                return NotFound();

            var dto = _mapper.Map<DiscountDto>(discount);
            return View(dto);
        }

        // POST: DeleteConfirmed/id - видалення знижки
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _discountService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
