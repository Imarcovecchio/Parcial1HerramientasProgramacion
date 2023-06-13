using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parcial.Data;

namespace Parcial.Controllers
{
    public class CategoriaController : Controller
    {

        private ICategoriaSerivces _categoriaServices;
        private IbookServices _bookServices;
        public CategoriaController(ICategoriaSerivces categoriaSerivces, IbookServices bookServices)
        {
            _categoriaServices =categoriaSerivces;
            _bookServices = bookServices;
        }

        public async Task<IActionResult> Index(string nameFilter)
        {
        var model = new CategoriaViewModel();
        
        model.Categorias = _categoriaServices.QuerySearch(nameFilter);
        return View(model);
        }
        

        public IActionResult CreateCategoria()
        {
            ViewData["Libros"] = _categoriaServices.ObtenerBooks();
            return View();
        }

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategoria([Bind("Id,Nombre,Descripcion")] CategoriaCreateViewModel categoriaView)
        {
            if (ModelState.IsValid)
            {
                var categoria = new Categoria
                {
                    Nombre = categoriaView.Nombre,
                    Descripcion = categoriaView.Descripcion,
                    Libros= new List<Book>()
                };

                _categoriaServices.Create(categoria);

                return RedirectToAction(nameof(Index));
            }
            return View(categoriaView);
        }
    
        // GET: Autor/Delete/5
        [Authorize(Roles="Administrador,Supervisor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = _categoriaServices.GetCategoria(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Autor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Administrador,Supervisor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var categoria = _categoriaServices.GetCategoria(id);
            if (categoria != null)
            {
                _categoriaServices.Delete(categoria);
            }
            return RedirectToAction(nameof(Index));
        }

             
        // GET: Autor/Edit/5
        [Authorize(Roles="Administrador,Supervisor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = _categoriaServices.GetCategoria(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Autor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Administrador,Supervisor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Libros")] CategoriaEditViewModel categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var categoria1 = new Categoria{
                    Id=categoria.Id,
                    Nombre=categoria.Nombre,
                    Descripcion= categoria.Descripcion,
                };           
                _categoriaServices.Update(categoria1);
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        [Authorize(Roles="Administrador,Supervisor,Lectura")]
        public IActionResult LibrosEnCategoria(int categoriaId)
        {
            var categoria = _categoriaServices.GetCategoria(categoriaId);
            if (categoria == null)
            {
                return NotFound();
            }

            var libros = _bookServices.GetByCategoriaId(categoriaId);

            var viewModel = new LibrosEnCategoriaViewModel
            {
                Categoria = categoria,
                Libros = libros
            };

            return View(viewModel);
        }
}
}