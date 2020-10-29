using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RockyOpet.Data;
using RockyOpet.Models;


// kad god nesto saljemo iz Kontrolera moramo unutar viewa to da prihvatimo


namespace RockyOpet.Controllers
{
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _db;


        public ProductController(ApplicationDbContext dbb)
        {
            _db = dbb;
        }



        public IActionResult Index()
        {

            IEnumerable<Product> objList = _db.Product;
            foreach (var obj in objList)
            {
                obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            };

            return View(objList);
        }


        // GET FOR UPSERT
        public IActionResult Upsert(int? id)     // ako je edit stice Id od producta   
        {
                    // iz kategorije nam treba za drop down za text ime i value za id
            IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            ViewBag.CategoryDropDown = CategoryDropDown;        // viewBagu dodijelimo ovo iznad za temporary data iz controllora u view

            Product product = new Product();

            if (id == null)
            {//this is for create
                return View(product);
            }
            else
            {
                product = _db.Product.Find(id);     // nadji taj producta
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }

        }

        //POST FOR UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);

        }





        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Category.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Category.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Category.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");


        }








    }
}
