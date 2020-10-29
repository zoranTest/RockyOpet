using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RockyOpet.Data;
using RockyOpet.Models;


// kad god nesto saljemo iz Kontrolera moramo unutar viewa to da prihvatimo


namespace RockyOpet.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _db;


        public CategoryController(ApplicationDbContext dbb)
        {
            _db = dbb;
        }


            
        public IActionResult Index()        // desni klik na index i add a View
        {

            IEnumerable<Category> objList = _db.Category;       // pokupi sve kategorije iz baze

            return View(objList);   // vratimo ovu listu da bude prikazana u viewu a u viewu imamo model koji to pokupi
        }


        // GET FOR CREATE
        public IActionResult Create()        
        {                  
            return View();   
        }

        //POST FOR CREATE
        [HttpPost]      // ovo je post action method od create.cshmtl viewa i forme
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)   // da dodamo u bazu novu kategoriju 
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");   // necemo view da vracamo nego redirect to action method a to je index iz ovog controllera
            }

            return View(obj);  // samo nazad na view da mozemo prikazati greske  a obj da ostanu podaci u inputimam
            
        }



        //GET - EDIT (da se otvori edit uopste)
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)  // ako nije stigao id
            {
                return NotFound();
            }
            var obj = _db.Category.Find(id);    // nadji u  bazi tog
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }



        //POST - EDIT  (da spasi izmjene ako je validno sve)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(obj);       // umjesto add ide update
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }











    }
}
