using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        [Authorize(Roles="Administrador,Alumno")]
         public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrador,Alumno")]
        public ActionResult VerLibrosDisponibles(string searchTerm)
        {
            var librosDisponibles = _bookServices.GetBooksAvailable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                if (Enum.TryParse(searchTerm, true, out GeneroType generoType))
                {
                    librosDisponibles = librosDisponibles.Where(libro => libro.Genero == generoType).ToList();
                }
                else if (int.TryParse(searchTerm, out int year))
                {
                    librosDisponibles = librosDisponibles.Where(libro => libro.Año == year).ToList();
                }
                else
                {
                    string searchTermLower = searchTerm.ToLower();
                    librosDisponibles = librosDisponibles.Where(libro =>
                        libro.Nombre.ToLower().Contains(searchTermLower) ||
                        libro.Editorial.ToLower().Contains(searchTermLower)
                    ).ToList();
                }
            }

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


        [Authorize(Roles="Administrador,Alumno")]
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

        [Authorize(Roles="Administrador,Alumno")]
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
        [Authorize(Roles="Administrador,Alumno")]
        public ActionResult ConfirmarReserva(int[] libroIds)
        {
            foreach (int libroId in libroIds)
            {
            var libro = _bookServices.GetById(libroId);

             if (libro != null)
            {
            libro.EstaReservado = true;
            _bookServices.Reservar(libro);
            }
             }
    
            return RedirectToAction("Index");
        }
        
        [Authorize(Roles="Administrador,Alumno")]
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
        [Authorize(Roles="Administrador,Alumno")]
        public ActionResult ConfirmarDevolucion(int[] libroIds)
        {
            foreach (int libroId in libroIds)
            {
                var libro = _bookServices.GetById(libroId);

                if (libro != null)
                {
                    libro.EstaReservado = false;
                    _bookServices.Devolver(libro);
                }
            }
        return RedirectToAction("Index");
        }

        public IActionResult DownloadTable()
        {
            List<LibrosEnCategoriaViewModel> model = GetYourTableData();

            StringBuilder sb = new StringBuilder();

            foreach (var categoria in model)
            {
                sb.AppendLine("Categoría: " + categoria.Categoria.Nombre);
                
                if (categoria.Libros.Count > 0)
                {
                    foreach (var libro in categoria.Libros)
                    {
                        sb.AppendLine("- " + libro.Nombre);
                    }
                }
                else
                {
                    sb.AppendLine("No hay libros en esta categoría.");
                }

                sb.AppendLine();
            }

            byte[] fileContents = Encoding.UTF8.GetBytes(sb.ToString());

            string fileName = "tabla.txt";
            string contentType = "text/plain";
            return File(fileContents, contentType, fileName);
        }

        private List<LibrosEnCategoriaViewModel> GetYourTableData()
        {
            var categorias = _bookServices.GetCategorias();
            var librosEnCategoriasViewModel = new List<LibrosEnCategoriaViewModel>();

            foreach (var categoria in categorias)
            {
                var librosEnCategorias = new LibrosEnCategoriaViewModel
                {
                    Categoria = categoria,
                    Libros = _bookServices.GetByCategoriaId(categoria.Id)
                };

                librosEnCategoriasViewModel.Add(librosEnCategorias);
            }

            return librosEnCategoriasViewModel;
        }





    }
}
