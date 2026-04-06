using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EFC.Models;
using EFC.Services;

namespace EFC.Controllers
{
    public class SupermarketsController : Controller
    {
        private readonly ISupermarketService _service;

        public SupermarketsController(ISupermarketService service)
        {
            _service = service;
        }

        // GET: Supermarkets
        public async Task<IActionResult> Index(int? pageNum)
        {
            int pageSize = 3;
            var supermarkets = await _service.GetAllAsync();

            var list = await PaginatedList<Supermarket>
                .CreateAsync(supermarkets, pageNum ?? 1, pageSize);

            return View(list);
        }

        // GET: Supermarkets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supermarket = await _service.GetByIdAsync(id.Value);
            if (supermarket == null)
            {
                return NotFound();
            }

            return View(supermarket);
        }

        // GET: Supermarkets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Supermarkets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address")] Supermarket supermarket)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(supermarket);
                return RedirectToAction(nameof(Index));
            }
            return View(supermarket);
        }

        // GET: Supermarkets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supermarket = await _service.GetByIdAsync(id.Value);
            if (supermarket == null)
            {
                return NotFound();
            }
            return View(supermarket);
        }

        // POST: Supermarkets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address")] Supermarket supermarket)
        {
            if (id != supermarket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(supermarket);
                }
                catch
                {
                    if (!await _service.ExistsAsync(supermarket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(supermarket);
        }

        // GET: Supermarkets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supermarket = await _service.GetByIdAsync(id.Value);
            if (supermarket == null)
            {
                return NotFound();
            }

            return View(supermarket);
        }

        // POST: Supermarkets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SupermarketExists(int id)
        {
            return await _service.ExistsAsync(id);
        }
    }
}
