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
        private readonly AutorContext _context;

        public BookController(IbookServices bookServices, AutorContext context)
        {
            _bookServices = bookServices;
            _context = context;
        }

        // GET: Book
        public async Task<IActionResult> Index(string nameFilter)
        {
            var model = new BookViewModel();
            model.Books = _bookServices.QuerySearch(nameFilter);
            
              return View(model);
            
        }

        public IActionResult Reservar(int id){
            var book = _bookServices.GetById(id);

            if (book == null)
            {
            return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Reservar")]
        public async Task<IActionResult> ReservarConfirmed(int id)
        {
        var book = _bookServices.GetById(id);

        if (book == null)
        {
            return NotFound(); 
        }

        if (book.EstaReservado)
        {
            return RedirectToAction("Index");  
        }

        book.EstaReservado = true;
        _bookServices.Reservar(book);  // Actualizar el libro en la base de datos

        return RedirectToAction("Index");  // Redireccionar a la vista principal después de reservar el libro
        }
        public IActionResult QuitarReserva(int id){
            var book = _bookServices.GetById(id);

            if (book == null)
            {
            return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("QuitarReserva")]
        public async Task<IActionResult> QuitarReservaConfirmed(int id)
        {
        var book = _bookServices.GetById(id);

        if (book == null)
        {
            return NotFound(); 
        }

        if (!book.EstaReservado)
        {
            return RedirectToAction("Index");  
        }

        book.EstaReservado = false;
        _bookServices.Reservar(book);  // Actualizar el libro en la base de datos

        return RedirectToAction("Index");  // Redireccionar a la vista principal después de reservar el libro
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["AutorId"] = _bookServices.GetAutoresSelectList();
            ViewData["Categorias"] = _bookServices.GetCategoriaSelectList();
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
            ViewData["Categorias"] = _bookServices.GetCategoriaSelectList();
           
            return View();
        
        }


        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AutorId,Nombre,Editorial,Año,Genero,EstaReservado,CategoriaIds")] BookCreateViewModel bookView)
        {
            
           var categorias = _context.Categoria.Where(x=> bookView.CategoriaIds.Contains(x.Id)).ToList();
            if (ModelState.IsValid)
            {
                var book = new Book{
                    Id = bookView.Id,
                    AutorId=bookView.AutorId,
                    Nombre=bookView.Nombre,
                    Editorial=bookView.Editorial,
                    Año=bookView.Año,
                    Genero=bookView.Genero,
                    EstaReservado=bookView.EstaReservado,
                    Categorias = categorias

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
            ViewData["Categorias"] = _bookServices.GetCategoriaSelectList();
            if (id == null )
            {
                return NotFound();
            }

            var book = _bookServices.GetById(id.Value);
            if (book == null)
            {
                return NotFound();
            }
             var bookEditViewModel = new BookEditViewModel
            {
                    Id = book.Id,
                    AutorId = book.AutorId,
                    Nombre = book.Nombre,
                    Editorial = book.Editorial,
                    Año = book.Año,
                    Genero = book.Genero,
                    EstaReservado = book.EstaReservado,
                    CategoriaIds = book.Categorias.Select(c => c.Id).ToList()
            };
        
            return View(bookEditViewModel);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Administrador,Supervisor")]
        public async Task<IActionResult> Edit([Bind("Id,AutorId,Nombre,Editorial,Año,Genero,EstaReservado,CategoriaIds")] BookEditViewModel bookView)
        {
            var categorias = _context.Categoria.Where(x=> bookView.CategoriaIds.Contains(x.Id)).ToList();
          
            if (ModelState.IsValid)
            {
                var book = new Book{
                    Id = bookView.Id,
                    AutorId=bookView.AutorId,
                    Nombre=bookView.Nombre,
                    Editorial=bookView.Editorial,
                    Año=bookView.Año,
                    Genero=bookView.Genero,
                    EstaReservado=bookView.EstaReservado,
                    Categorias=categorias
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
