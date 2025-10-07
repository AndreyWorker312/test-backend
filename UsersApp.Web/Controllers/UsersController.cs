using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UsersApp.Application.Users;
using UsersApp.Application.Users.Dtos;

namespace UsersApp.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _service;

        public UsersController(IUserService service) => _service = service;

        // GET: /Users?q=term
        public async Task<IActionResult> Index(string? q)
        {
            ViewData["Query"] = q;
            var users = await _service.ListAsync(q);
            return View(users);
        }

        // GET: /Users/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _service.GetAsync(id);
            if (user is null) return NotFound();
            return View(user);
        }

        // GET: /Users/Create
        public IActionResult Create() => View();

        // POST: /Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserRequest model)
        {
            if (!ModelState.IsValid) return View(model);

            var (ok, error, user) = await _service.CreateAsync(model);
            if (!ok)
            {
                if (error?.Contains("Email") == true)
                    ModelState.AddModelError(nameof(CreateUserRequest.Email), error!);
                else if (error?.Contains("Phone") == true)
                    ModelState.AddModelError(nameof(CreateUserRequest.Phone), error!);
                else
                    ModelState.AddModelError(string.Empty, error ?? "Validation error");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Users/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _service.GetAsync(id);
            if (user is null) return NotFound();

            var vm = new UpdateUserRequest
            {
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address
            };
            ViewBag.UserId = user.Id;
            return View(vm);
        }

        // POST: /Users/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateUserRequest model)
        {
            if (!ModelState.IsValid) { ViewBag.UserId = id; return View(model); }

            var (ok, error, updated) = await _service.UpdateAsync(id, model);
            if (!ok)
            {
                if (error?.Contains("Email") == true)
                    ModelState.AddModelError(nameof(UpdateUserRequest.Email), error!);
                else if (error?.Contains("Phone") == true)
                    ModelState.AddModelError(nameof(UpdateUserRequest.Phone), error!);
                else
                    ModelState.AddModelError(string.Empty, error ?? "Validation error");
                ViewBag.UserId = id;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Users/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _service.GetAsync(id);
            if (user is null) return NotFound();
            return View(user);
        }

        // POST: /Users/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var (ok, _) = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
