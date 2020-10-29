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







    }
}
