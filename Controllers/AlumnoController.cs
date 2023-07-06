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

        public ActionResult ReservarLibro()
        {
            var librosDisponibles = _bookServices.GetBooksAvailable();
            var bookViewModel = new BookViewModel
            {
                Books = librosDisponibles
            };
            return View(bookViewModel);
        }


        [HttpPost]
        public ActionResult ConfirmarReserva(int libroId)
        {
            var libro = _bookServices.GetById(libroId);

            if (libro != null)
            {
                libro.EstaReservado = true;
                _bookServices.Reservar(libro);
                return RedirectToAction("Index");
            }

            return RedirectToAction("ReservarLibro");
        }
        
        public ActionResult DevolverLibro()
        {
            var librosDisponibles = _bookServices.GetBooksUnAvailable();
            var bookViewModel = new BookViewModel
            {
                Books = librosDisponibles
            };
            return View(bookViewModel);
        }


        [HttpPost]
        public ActionResult ConfirmarDevolucion(int libroId)
        {
            var libro = _bookServices.GetById(libroId);

            if (libro != null)
            {
                libro.EstaReservado = true;
                _bookServices.Devolver(libro);
                return RedirectToAction("Index");
            }

            return RedirectToAction("DevolverLibro");
        }




    }
}
