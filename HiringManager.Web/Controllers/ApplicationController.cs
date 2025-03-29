using Microsoft.AspNetCore.Mvc;
using HiringManager.Models;
using HiringManager.DataAccess.Data;
using HiringManager.DataAccess.Repository.IRepository;
using HiringManager.DataAccess.Repository;

namespace HiringManager.Web.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IUnitOfWork _db;
        public ApplicationController(IUnitOfWork db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<ApplicationDetail> obj = _db.Application.GetAll().ToList();
            return View(obj);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ApplicationDetail db)
        {
            if (ModelState.IsValid)
            {
                _db.Application.Add(db);
                _db.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id==null || id == 0)
            {
                return NotFound();
            }
            ApplicationDetail? obj = _db.Application.Get(u=>u.Id==id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(ApplicationDetail obj)
        {
            if (ModelState.IsValid)
            {
                _db.Application.update(obj);
                _db.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            ApplicationDetail? obj = _db.Application.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult DeletePOST(int? id)
        {
            ApplicationDetail? obj = _db.Application.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Application.Remove(obj);
            _db.Save();
            return RedirectToAction("Index");
        }
    }
}
