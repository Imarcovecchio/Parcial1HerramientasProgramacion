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
    public class ProfesorController : Controller
    {

        private readonly IbookServices _bookServices;
        
        
        public ProfesorController(IbookServices bookServices)
        {
            _bookServices =bookServices;
        }

        // GET: Autor
         public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdministrarAutores(){
           
           return RedirectToAction("Index", "Autor");
        }
        public ActionResult AdministrarCategorias(){
           
           return RedirectToAction("Index", "Categoria");
        }
        public ActionResult AdministrarLibros(){
           
           return RedirectToAction("Index", "Book");
        }
        public ActionResult AdministrarUsuarios(){
           
           return RedirectToAction("Index", "Users");
        }
        public ActionResult AdministrarRoles(){
           
           return RedirectToAction("Index", "Roles");
        }
    }
}