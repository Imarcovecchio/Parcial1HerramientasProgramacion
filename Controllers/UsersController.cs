using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Parcial.Models;

namespace Parcial.Controllers;

public class UsersController : Controller
    {
        
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersController(
        ILogger<HomeController> logger,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }


    public IActionResult Index()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

   [Authorize(Roles="Administrador")]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        
        var userViewModel = new UserEditViewModel();
        userViewModel.UserName = user.UserName ?? string.Empty;
        userViewModel.Email = user.Email ?? string.Empty;
        userViewModel.Roles = new SelectList(_roleManager.Roles.ToList());
        var usuarios =  _userManager.Users.ToList();
        userViewModel.UserNames = usuarios.Select(r=> new SelectListItem{Value= r.UserName, Text = r.UserName});
        return View(userViewModel);
    }
    
    [Authorize(Roles="Administrador")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
     [HttpPost]
    [Authorize(Roles="Administrador")]
    public async Task<IActionResult> Edit(UserEditViewModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);

        if(model.Role == "Ninguno"){
            ModelState.AddModelError("Role", "Ingrese un rol válido");
        }
        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, model.Role);
            
        }

        return RedirectToAction("Index");
    }

    }

    
