using birthreg.Models;
using birthreg.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace birthreg.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return RedirectToAction("login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            var (error, user) = await _userService.Login(model.Email, model.Password);
            if (user == null)
            {
                TempData["ErrorMsg"] = error;
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                HttpContext.Session.SetString("JWToken", error);
                return RedirectToAction("Dashboard", "Registrar");
            }
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Login");
        }
    }
}
