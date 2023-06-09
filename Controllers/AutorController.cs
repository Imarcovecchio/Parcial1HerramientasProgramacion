using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Parcial.Data;

namespace Parcial.Controllers
{
    public class AutorController : Controller
    {
        private readonly AutorContext _context;

        public AutorController(AutorContext context)
        {
            _context = context;
        }

        // GET: Autor
        public async Task<IActionResult> Index(string nameFilter)
        {
            var query = from autor in _context.Autor select autor;
            if(!string.IsNullOrEmpty(nameFilter)){
                query = query.Where(x => x.Nombre.ToLower().Contains(nameFilter.ToLower()) || x.Apellido.ToLower().Contains(nameFilter.ToLower()) || x.Edad.ToString() == nameFilter);
            }

            var model = new AutorViewModel();
            model.Autors = await query.ToListAsync();

            return _context.Autor != null ? 
                          View(model) :
                          Problem("Entity set 'AutorContext.Book'  is null.");


        }

        // GET: Autor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Autor == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // GET: Autor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Edad,Genero")] AutorCreateViewModel autorView)
        {
            if (ModelState.IsValid)
            {
                var autor = new Autor{
                    Id=autorView.Id,
                    Nombre=autorView.Nombre,
                    Apellido=autorView.Apellido,
                    Edad=autorView.Edad,
                    Genero=autorView.Genero
                };
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autorView);
        }

        // GET: Autor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Autor == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // POST: Autor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Edad,Genero")] AutorEditViewModel autorView)
        {
            if (id != autorView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var autor = new Autor{
                    Id=autorView.Id,
                    Nombre=autorView.Nombre,
                    Apellido=autorView.Apellido,
                    Edad=autorView.Edad,
                    Genero=autorView.Genero
                };

                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.Id))
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
            return View(autorView);
        }

        // GET: Autor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Autor == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // POST: Autor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Autor == null)
            {
                return Problem("Entity set 'AutorContext.Autor'  is null.");
            }
            var autor = await _context.Autor.FindAsync(id);
            if (autor != null)
            {
                _context.Autor.Remove(autor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
          return (_context.Autor?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
