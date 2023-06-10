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
        private readonly IbookServices _bookServices;

        public BookController(IbookServices bookServices)
        {
            _bookServices = bookServices;
        }

        

        // GET: Book
        public async Task<IActionResult> Index(string nameFilter)
    {
            var model = new BookViewModel();
            model.Books = _bookServices.QuerySearch(nameFilter);
            
              return View(model);
            
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["AutorId"] = _bookServices.GetAutoresSelectList();
            if (id == null )
            {
                return NotFound();
            }

            var book = _bookServices.GetById(id.Value);
            
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            ViewData["AutorId"] = _bookServices.GetAutoresSelectList();
            // Obtener la lista de autores desde alguna fuente de datos
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
                _bookServices.Create(book);
                return RedirectToAction(nameof(Index));
            }
            
            return View(bookView);
        }

        // GET: Book/Edit/5
        [Authorize(Roles="Administrador,Supervisor")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["AutorId"] = _bookServices.GetAutoresSelectList();
            if (id == null )
            {
                return NotFound();
            }

            var book = _bookServices.GetById(id.Value);
            if (book == null)
            {
                return NotFound();
            }
            
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Administrador,Supervisor")]
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
                _bookServices.Update(book);
                return RedirectToAction(nameof(Index));
            }
            
            return View(bookView);
        }
        [Authorize(Roles="Administrador,Supervisor")]
        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["AutorId"] = _bookServices.GetAutoresSelectList();
            if (id == null )
            {
                return NotFound();
            }

            var book = _bookServices.GetById(id.Value);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Administrador,Supervisor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var book = _bookServices.GetById(id);
            if (book != null)
            {
                _bookServices.Delete(book);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return _bookServices.GetById(id) != null;
        }
    }
}
