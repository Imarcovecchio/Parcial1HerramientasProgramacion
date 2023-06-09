using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Parcial.Data;

namespace Parcial.Controllers
{
    public class ProfesorController : Controller
    {
      private readonly ILogger<HomeController> _logger;
      private readonly UserManager<IdentityUser> _userManager;
      private readonly RoleManager<IdentityRole> _roleManager;

    public ProfesorController (
        ILogger<HomeController> logger,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }

        // GET: Autor
        [Authorize(Roles="Administrador,Profesor")]
         public ActionResult Index()
        {
            return View();
        }

         [Authorize(Roles="Administrador,Profesor")]
        public async Task<ActionResult> UsuariosConRoles()
         {
            var usuarios = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();
            var usuariosConRoles = new List<UsuariosConRolesViewModel>();

               foreach (var usuario in usuarios)
               {
                  var usuarioRoles = await _userManager.GetRolesAsync(usuario);
                  var rolAsignado = roles.FirstOrDefault(r => usuarioRoles.Contains(r.Name));

                  usuariosConRoles.Add(new UsuariosConRolesViewModel
                  {
                        Usuario = usuario,
                        Rol = rolAsignado != null ? rolAsignado.Name : "Sin rol asignado"
                  });
               }

               return View(usuariosConRoles);

         }

       [Authorize(Roles="Administrador,Profesor")]
        public ActionResult AdministrarAutores(){
           
           return RedirectToAction("Index", "Autor");
        }

        [Authorize(Roles="Administrador,Profesor")]
        public ActionResult AdministrarCategorias(){
           
           return RedirectToAction("Index", "Categoria");
        }

        [Authorize(Roles="Administrador,Profesor")]
        public ActionResult AdministrarLibros(){
           
           return RedirectToAction("Index", "Book");
        }

        [Authorize(Roles="Administrador,Profesor")]
        public ActionResult AdministrarUsuarios(){
           
           return RedirectToAction("Index", "Users");
        }

        [Authorize(Roles="Administrador,Profesor")]
        public ActionResult AdministrarRoles(){
           
           return RedirectToAction("Index", "Roles");
        }
    }
}