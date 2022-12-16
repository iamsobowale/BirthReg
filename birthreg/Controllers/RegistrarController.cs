using birthreg.Models;
using birthreg.Services;
using birthreg.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace birthreg.Controllers
{
    public class RegistrarController : Controller
    {
        private readonly IChildService _childService;
        private readonly IParentService _parentService;
        private readonly IUserService _userService;

        public RegistrarController(IChildService childService, IParentService parentService, IUserService userService)
        {
            _childService = childService;
            _parentService = parentService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> Dashboard()
        {
            var auth = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(auth))
            {
                return RedirectToAction("Login", "Auth");
            }
            var statistics = await _childService.GetStatistics();   
            return View(statistics);
        }

        public async Task<IActionResult> Children(string searchString = "")
        {
            var auth = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(auth))
            {
                return RedirectToAction("Login", "Auth");
            }
            var children = await _childService.GetChildren(searchString);
            return View(children);
        }


        [HttpGet]
        public async Task<IActionResult> AddChild()
        {
            var auth = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(auth))
            {
                return RedirectToAction("Login", "Auth");
            }
            var parents = await PopulateParentsDropDownList();
            ViewBag.ParentId = new SelectList(parents, "Id", "FamilyName");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddChild(AddChildViewModel child)
        {
            var newChild = await _childService.AddChild(child);
            return RedirectToAction("Children");
        }

        public async Task<IActionResult> Parents(string searchString = "")
        {
            var parents = await _parentService.GetAllParent(searchString);
            return View(parents);
        }

        [HttpGet]
        public IActionResult AddParent()
        {
            var auth = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(auth))
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddParent(Parent parent)
        {
            var newParent = await _parentService.AddParent(parent);
            return RedirectToAction("Parents");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            var auth = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(auth))
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var (response, msg) = await _userService.ChangePasswordAsync(model);
            if (response)
            {
                TempData["Success"] = msg;
                return View();
            }
            else
                TempData["Error"] = msg;
            return View();
        }

        [HttpGet]
        public IActionResult RetrieveCertificate()
        {
            var auth = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(auth))
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RetrieveCertificate(string certNo)
        {
            var child = await _childService.GetChildByCertNo(certNo);
            return View(child);
        }

        [HttpPost]
        public async Task<IActionResult> GetChild(int id)
        {
            var child = await _childService.GetChildById(id);
            return View(child);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteChild(int id)
        {
            await _childService.DeleteChild(id);
            return RedirectToAction("Children");
        }



        private async Task<IEnumerable<Parent>> PopulateParentsDropDownList(object selectedParent = null)
        {
            var parents = await _parentService.GetAllParent();
            return parents;
        }


    }
}
