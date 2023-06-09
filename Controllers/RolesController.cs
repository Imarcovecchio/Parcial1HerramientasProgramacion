using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Parcial.Models;

namespace Parcial.Controllers;

    public class RolesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(
        ILogger<HomeController> logger,
        RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        //listar todos los roles
        var roles = _roleManager.Roles.ToList();
        return View(roles);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles="Administrador")]
    public IActionResult Create(RoleCreateViewModel model)
    {
        if(string.IsNullOrEmpty(model.RoleName))
        {
            return View();
        }

        var role = new IdentityRole(model.RoleName);
        _roleManager.CreateAsync(role);

        return RedirectToAction("Index");
    }

    
    [Authorize(Roles="Administrador")]
    public async Task<IActionResult> Edit(string id)
    {
        var rol = await _roleManager.FindByIdAsync(id);
        
        var roleViewModel = new RoleEditViewModel();
        roleViewModel.RoleName = rol.Name ?? string.Empty;
        roleViewModel.NewRoleName=rol.Name?? string.Empty;

        var roles = await _roleManager.Roles.ToListAsync();
        roleViewModel.RoleNames = roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name });

        return View(roleViewModel);
    }

     [HttpPost]
     [Authorize(Roles="Administrador")]
    public async Task<IActionResult> Edit(RoleEditViewModel model)
    {
        var rol = await _roleManager.FindByNameAsync(model.RoleName);
       
        if (rol != null)
        {
            rol.Name=model.NewRoleName;
            await _roleManager.UpdateAsync(rol);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles="Administrador")]
    public async Task<IActionResult> Delete(string id)
    {
        var rol = await _roleManager.FindByIdAsync(id);

        var roleViewModel = new RoleEditViewModel();
        roleViewModel.RoleName = rol != null ? rol.Name : string.Empty;

        var roles = await _roleManager.Roles.ToListAsync();
        roleViewModel.RoleNames = roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name });
        return View(roleViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles="Administrador")]
    public async Task<IActionResult> Delete(RoleCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "El rol no existe.");
                return View(model);
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }
    }
