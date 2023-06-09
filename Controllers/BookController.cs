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
    public class BookController : Controller
    {
        private readonly AutorContext _context;

        public BookController(AutorContext context)
        {
            _context = context;
        }

        // GET: Book
        public async Task<IActionResult> Index(string nameFilter)
    {
            var query = from book in _context.Book select book; 
            if (!string.IsNullOrEmpty(nameFilter))
            {
                query = query.Where(x=> x.Nombre.ToLower().Contains(nameFilter.ToLower()) || x.Editorial.ToLower().Contains(nameFilter.ToLower()) || x.Año.ToString() == nameFilter);
            }

            var queryReady = await query.Include(b =>b.Autor).ToListAsync();

//
            var model = new BookViewModel();
            model.Books = queryReady;
            
              return _context.Book != null ? 
                          View(model) :
                          Problem("Entity set BookContext.Book is null");
            
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Autor)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            // Obtener la lista de autores desde alguna fuente de datos
            ViewData["AutorId"] = new SelectList(_context.Autor, "Id", "Nombre");
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AutorId,Nombre,Editorial,Año,Genero,EstaReservado")] BookCreateViewModel bookView)
        {
            if (ModelState.IsValid)
            {
                var book = new Book{
                    Id = bookView.Id,
                    AutorId=bookView.AutorId,
                    Nombre=bookView.Nombre,
                    Editorial=bookView.Editorial,
                    Año=bookView.Año,
                    Genero=bookView.Genero,
                    EstaReservado=bookView.EstaReservado

                };
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AutorId"] = new SelectList(_context.Autor, "Nombre", "Id", bookView.AutorId);
            return View(bookView);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AutorId"] = new SelectList(_context.Autor, "Id", "Nombre", book.AutorId);
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AutorId,Nombre,Editorial,Año,Genero,EstaReservado")] BookEditViewModel bookView)
        {
            if (id != bookView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var book = new Book{
                    Id = bookView.Id,
                    AutorId=bookView.AutorId,
                    Nombre=bookView.Nombre,
                    Editorial=bookView.Editorial,
                    Año=bookView.Año,
                    Genero=bookView.Genero,
                    EstaReservado=bookView.EstaReservado

                };

                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            
            return View(bookView);
        }
        [Authorize(Roles="Administrador")]
        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Autor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'AutorContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
