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
        //listar todos los roles
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
    


             

}
}