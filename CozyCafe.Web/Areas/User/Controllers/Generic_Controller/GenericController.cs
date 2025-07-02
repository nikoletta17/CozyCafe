using CozyCafe.Application.Interfaces.Generic_Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Areas.User.Controllers.Generic_Controller
{
    public class GenericController<T> : Controller where T : class
    {
        protected readonly IService<T> _service;
        public GenericController(IService<T> service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var items = await _service.GetAllAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var items = await _service.GetByIdAsync(id);
            if(items == null)
            {
                return NotFound();
            }
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(); 
            /// Повернути форму створення
            ///dropdown списки(категорії тощо), тут їх можна буде завантажувати.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(T entity)
        {
            if(ModelState.IsValid)
            {
                await _service.AddAsync(entity);
                return RedirectToAction(nameof(Index)); 
            }
            return View(entity);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(T entity, int id)
        {
            if(ModelState.IsValid)
            {
                await _service.UpdateAsync(entity);
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if(entity != null)
            {   
                await _service.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));

        }
    }
}
