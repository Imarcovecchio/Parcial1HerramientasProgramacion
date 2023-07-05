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
    public class AlumnoController : Controller
    {

        private readonly IbookServices _bookServices;
        
        
        public AlumnoController(IbookServices bookServices)
        {
            _bookServices =bookServices;
        }

        // GET: Autor
         public ActionResult Index()
        {
            return View();
        }

        public ActionResult VerLibrosDisponibles(){
            var librosDisponibles = _bookServices.GetBooksAvailable();

            var librosDisponiblesViewModel = librosDisponibles.Select(libro => new BookCreateViewModel
            {
            Id = libro.Id,
            AutorId = libro.AutorId,
            Nombre = libro.Nombre,
            Editorial = libro.Editorial,
            Año = libro.Año,
            Genero = libro.Genero,
            EstaReservado = libro.EstaReservado,
            }).ToList();
            return View(librosDisponiblesViewModel);
        }

        public ActionResult VerCategorias(){
            var categorias = _bookServices.GetCategorias();
            var librosEnCategoriasViewModel = new List<LibrosEnCategoriaViewModel>();
            foreach(var categoria in categorias)
            {
                var librosEnCategorias = new LibrosEnCategoriaViewModel
                {
                    Categoria=categoria,
                    Libros= _bookServices.GetByCategoriaId(categoria.Id)
                };
                librosEnCategoriasViewModel.Add(librosEnCategorias);
            }

            return View(librosEnCategoriasViewModel);
        }

    }
}
