using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RockyOpet.Data;
using RockyOpet.Models;
using RockyOpet.Models.ViewModels;


// kad god nesto saljemo iz Kontrolera moramo unutar viewa to da prihvatimo


namespace RockyOpet.Controllers
{
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _db;

        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductController(ApplicationDbContext dbb, IWebHostEnvironment webHostEnvironment)
        {
            _db = dbb;
            _webHostEnvironment = webHostEnvironment;
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
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            //// ViewBag.CategoryDropDown = CategoryDropDown;        // viewBagu dodijelimo ovo iznad za temporary data iz controllora u view
            //ViewData["CategoryDropDown"] = CategoryDropDown;

            //Product product = new Product();

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            ProductVM productVM = new ProductVM() { Product = new Product(), CategorySelectList = _db.Category.Select(i => new SelectListItem { Text=i.Name,Value=i.Id.ToString() }) };


            if (id == null)
            {//this is for create
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Product.Find(id);     // iz vm-a product
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }

        }

        //POST FOR UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;     // prvo nam treba ta uploadana slika
                string webRootPath = _webHostEnvironment.WebRootPath;   // treba nam path od wwwroot

                if (productVM.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;     // wwwroot+\images\product
                    string fileName = Guid.NewGuid().ToString();    // generisemo naziv fajla
                    string extension = Path.GetExtension(files[0].FileName);    // jpg,png, tip slike

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))    // kreiraj novi fajl image i spasi gdje treba
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension; // ovde samo string pohranimo sa adresom i nazivom slike + ekstenzijom

                    _db.Product.Add(productVM.Product); // dodaj u bazu
                }
                else
                {
                    //updating
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);       //!! asNoTracking jako bitno da baza povuce ali ne prati vise i da moze dalje ici using...

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }
                    _db.Product.Update(productVM.Product);
                }


                _db.SaveChanges();      // spasi bazu
                return RedirectToAction("Index");
            }

            productVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(productVM);

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
