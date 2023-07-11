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
        private IAutorServices _autorServices;
        public AutorController(IAutorServices autorServices)
        {
            _autorServices =autorServices;
        }

        // GET: Autor
        public async Task<IActionResult> Index(string nameFilter)
        {

            var model = new AutorViewModel();
            model.Autors = _autorServices.QuerySearch(nameFilter);

            return View(model);
                          
        }

        // GET: Autor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = _autorServices.GetById(id.Value);
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
                _autorServices.Create(autor);
                return RedirectToAction(nameof(Index));
            }
            return View(autorView);
        }

        // GET: Autor/Edit/5
        [Authorize(Roles="Administrador,Profesor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = _autorServices.GetById(id.Value);
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
        [Authorize(Roles="Administrador,Profesor")]
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
                _autorServices.Update(autor);
                return RedirectToAction(nameof(Index));
            }
            return View(autorView);
        }

        // GET: Autor/Delete/5
        [Authorize(Roles="Administrador,Profesor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = _autorServices.GetById(id.Value);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // POST: Autor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Administrador,Profesor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var autor = _autorServices.GetById(id);
            if (autor != null)
            {
                _autorServices.Delete(autor);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
          return _autorServices.GetById(id) != null;
        }
    }
}
